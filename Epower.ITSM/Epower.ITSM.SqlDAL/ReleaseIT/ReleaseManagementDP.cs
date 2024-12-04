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
using EpowerCom;

namespace Epower.ITSM.SqlDAL.Service
{
    /// <summary>
    /// 
    /// </summary>
    public class ReleaseManagementDP
    {
        /// <summary>
        /// 
        /// </summary>
        public ReleaseManagementDP()
        { }

        /// <summary>获取发布版本的信息
        /// 
        /// </summary>
        /// <param name="where"></param>
        /// <param name="lngUserID"></param>
        /// <param name="lngDeptID"></param>
        /// <param name="lngOrgID"></param>
        /// <param name="re"></param>
        /// <param name="pagesize"></param>
        /// <param name="pageindex"></param>
        /// <param name="rowcount"></param>
        /// <returns></returns>
        public static DataTable GetReleaseInfor(string where, long lngUserID, long lngDeptID, long lngOrgID, RightEntity re, int pagesize, int pageindex, ref int rowcount)
        { 
            string strTmp = "";
            string status = "-1";
            string versionname = "";
            string versioncode = "";
            string releasescopeid = "-1";
            string versionkindid = "-1";
            string versiontypeid = "-1";
            string releasepersonid = "0";
            string regbegin = string.Empty;
            string regend = string.Empty;

            string strWhere = " 1=1 ";
 
            XmlTextReader tr = new XmlTextReader(new StringReader(where));
            while (tr.Read())
            {
                if (tr.Name == "Field" && tr.NodeType == XmlNodeType.Element)
                {
                    strTmp = tr.GetAttribute("Value").Trim();
                    switch (tr.GetAttribute("FieldName"))
                    {
                    case "status":
                            status = strTmp; 
                    break; 
                    case "versionname":
                             versionname = strTmp; 
                    break; 
                    case "versioncode":
                             versioncode = strTmp; 
                    break; 
                    case "releasescopeid":
                             releasescopeid = strTmp; 
                    break; 
                    case "versionkindid":
                             versionkindid = strTmp; 
                    break; 
                    case "versiontypeid":
                             versiontypeid = strTmp; 
                    break; 
                    case "releasepersonid":
                             releasepersonid = strTmp; 
                    break; 
                        case "RegBegin":
                            regbegin = strTmp;
                            break;
                        case "RegEnd":
                            regend = strTmp;
                            break;
                        default:
                            break;
                    }
                }
            }
            tr.Close(); 

            string sWhere = "";
            int iRowCount = 0;
            if (status != "-1")
            {
                sWhere += " and status =" + status;
            }
            if (versionname != string.Empty)
            {
                sWhere += " and versionname like " + StringTool.SqlQ("%" + versionname + "%");
            }
            if (versioncode != string.Empty)
            {
                sWhere += " and versioncode like " + StringTool.SqlQ("%" + versioncode + "%");
            }
            if (releasescopeid !="-1")
            {
                sWhere += " and releasescopeid= " + releasescopeid;
            }
            if (versionkindid != "-1")
            {
                sWhere += " and versionkindid = " + versionkindid;
            }
            if (versiontypeid != "-1")
            {
                sWhere += " and versiontypeid = " + versiontypeid;
            }
            if (releasepersonid !="0")
            {
                sWhere += " and releasepersonid = " + releasepersonid;
            }
            if(!string.IsNullOrEmpty ( regbegin ))
                sWhere += " And releasedate >=" + StringTool.SqlQ(regbegin + " 00:00:00" ) ;
            if(!string.IsNullOrEmpty (regend ))
                sWhere += " And releasedate < " + StringTool.SqlQ(regend + " 00:00:00" ); 


            string strList = "";
            if (re != null )
            {
                if (re == null || re.CanRead == false)
                {
                    //查询出空结果
                    strWhere += " AND 1 = -1 ";
                }
                else if (re != null && re.CanRead == true)
                {
                    #region 范围条件
                    switch (re.RightRange)
                    {
                        case eO_RightRange.eFull:
                            strWhere += "";
                            break;
                        case eO_RightRange.ePersonal:
                            strWhere += "AND exists (SELECT messageid FROM es_message WHERE flowid = V_RELEASEINFOR.flowid AND receiverid = " + lngUserID.ToString() + ")";
                            break;
                        case eO_RightRange.eDeptDirect:
                            strWhere += "AND exists (SELECT messageid FROM es_message WHERE flowid = V_RELEASEINFOR.flowid AND recdeptid = " + lngDeptID.ToString() + ")";
                            break;
                        case eO_RightRange.eOrgDirect:
                            strWhere += "AND exists (SELECT messageid FROM es_message WHERE flowid = V_RELEASEINFOR.flowid AND recorgid = " + lngOrgID.ToString() + ")";
                            break;
                        case eO_RightRange.eDept:
                            strList = DeptDP.GetDeptFullID(lngDeptID);
                            if (strList.Trim().Length > 0)
                            {
                                //不是根部门并有找到
                                strWhere += "AND exists (SELECT messageid FROM es_message WHERE flowid = V_RELEASEINFOR.flowid AND recdeptid in (select deptid from ts_dept where fullid like "+StringTool.SqlQ(  strList+ " % ")+"))";
                            }
                            break;
                        case eO_RightRange.eOrg:
                            strList = DeptDP.GetDeptFullID(lngOrgID);
                            if (strList.Trim().Length > 0)
                            {
                                //不是根部门并有找到
                                strWhere += "AND exists (SELECT messageid FROM es_message WHERE flowid = V_RELEASEINFOR.flowid AND recorgid in (select distinct deptid from ts_dept where deptkind = 1 and fullid like "+StringTool.SqlQ(  strList+ " % ")+"))";
                            }
                            break;
                        default:
                            strWhere += "";
                            break;
                    }
                    #endregion
                }
            }
            OracleConnection cn = ConfigTool.GetConnection("SQLConnString");
            //DataTable dt = OracleDbHelper.ExecuteDataTable(cn, CommandType.Text, strSQL + where + strWhere);
            //DataTable dt = OracleDbHelper.ExecuteDataTable(cn, "OA_RELEASEMANAGEMENT a , ts_user b , es_flow c , es_nodemodel d", "a.flowid,c.status,DATEDIFF('Minute', sysdate, nvl(c.expectendtime, sysdate)) AS FlowDiffMinute,a.versionname,a.versioncode,a.releasedate,a.releasescopename , a.releasepersonname,a.versionkindname,a.versiontypename,a.releasephone ,case c.Status when 20 then '正在处理' when 40 then '流程暂停' when 50 then '流程终止' else '正常结束' end as flowStatus", " ORDER BY RMID DESC", pagesize, pageindex, strWhere + where, ref rowcount);
            DataTable dt = OracleDbHelper.ExecuteDataTable(cn, "V_RELEASEINFOR", "*", " ORDER BY RMID DESC", pagesize, pageindex, strWhere +sWhere  , ref rowcount);
            ConfigTool.CloseConnection(cn);
            return dt;
        }

        /// <summary>保存子版本的信息
        /// 
        /// </summary>
        /// <param name="dt"></param>
        /// <param name="lngID"></param>
        /// <returns></returns>
        public static long SaveSubVersionItem(DataTable dt, long lngID)
        {
            OracleConnection cn = ConfigTool.GetConnection();
            if (cn.State != ConnectionState.Open)
            {
                cn.Open();
            }
            OracleTransaction tran = cn.BeginTransaction();
            try
            {
                if (lngID != 0)
                {
                    DeleteCLFareDetailItem(lngID, tran); //先删除再新增
                }
                else
                {
                    lngID = EPGlobal.GetNextID("OA_RELEASEMANAGEMENTID");
                }
                if (dt.Rows.Count < 1)
                {
                    tran.Commit();
                    return lngID;
                }

                for (int n = 0; n < dt.Rows.Count; n++)
                {
                    dt.Rows[n]["SMSID"] = lngID;
                }
                string sSql = "SELECT * FROM OA_RELEASESUB Where SMSID=" + lngID;
                OracleDataAdapter da = new OracleDataAdapter(sSql, cn);
                OracleCommandBuilder cb = new OracleCommandBuilder(da);
                da.SelectCommand.Transaction = tran;
                cb.GetInsertCommand().Transaction = tran;
                da.Update(dt);

                tran.Commit();

                return lngID;
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

        /// <summary>删除
        /// 
        /// </summary>
        /// <param name="strNo"></param>
        /// <param name="tran"></param>
        public static void DeleteCLFareDetailItem(long lngID, OracleTransaction  tran)
        {

            string sSql = "delete from oa_releasesub where SMSID = " + lngID.ToString();//ID , rmid
            try
            { 
                OracleDbHelper.ExecuteNonQuery(tran, CommandType.Text, sSql);
            }
            catch (Exception e)
            {
                throw e;
            }
        }

        /// <summary>获取子版本信息
        /// 
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public static DataTable GetCLFareItem(long id)
        {
            string sSql = @"SELECT * FROM OA_RELEASESUB WHERE SMSID = " + id.ToString();

            OracleConnection  cn = ConfigTool.GetConnection();
            try
            {
                return OracleDbHelper.ExecuteDataTable(cn, CommandType.Text, sSql);                  
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
    }
}
