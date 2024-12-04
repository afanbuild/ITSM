/*******************************************************************
 *
 * Description:资产关联
 * 
 * 
 * Create By  :zhumc
 * Create Date:2008年4月23日
 * *****************************************************************/
using System;
using System.Data;
using System.Xml;
using System.Data.OracleClient;
using Epower.DevBase.BaseTools;
using Epower.ITSM.Base;
using Epower.DevBase.Organization.SqlDAL;
using Epower.DevBase.Organization.Base;
using Epower.DevBase.BaseTools;
using System.Text;

namespace Epower.ITSM.SqlDAL
{
    /// <summary>
    /// 
    /// </summary>
    public class Equ_RelDP
    {
        /// <summary>
        /// 
        /// </summary>
        public Equ_RelDP()
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

        #region Equ_ID
        /// <summary>
        ///
        /// </summary>
        private Decimal mEqu_ID;
        public Decimal Equ_ID
        {
            get { return mEqu_ID; }
            set { mEqu_ID = value; }
        }
        #endregion

        #region 关联资产ID
        /// <summary>
        ///
        /// </summary>
        private Decimal mRelID;
        public Decimal RelID
        {
            get { return mRelID; }
            set { mRelID = value; }
        }
        #endregion

        #region 关联资产属性ID
        /// <summary>
        ///
        /// </summary>
        private Decimal mRelPropID;
        public Decimal RelPropID
        {
            get { return mRelPropID; }
            set { mRelPropID = value; }
        }
        #endregion

        #region RelPropName
        /// <summary>
        ///
        /// </summary>
        private String mRelPropName = string.Empty;
        public String RelPropName
        {
            get { return mRelPropName; }
            set { mRelPropName = value; }
        }
        #endregion

        #region RelDescription
        /// <summary>
        ///
        /// </summary>
        private String mRelDescription = string.Empty;
        public String RelDescription
        {
            get { return mRelDescription; }
            set { mRelDescription = value; }
        }
        #endregion

        #endregion

        #region GetReCorded
        /// <summary>
        /// 
        /// </summary>
        /// <param name="lngID"></param>
        /// <returns>Equ_RelDP</returns>
        public Equ_RelDP GetReCorded(long lngID)
        {
            Equ_RelDP ee = new Equ_RelDP();
            string strSQL = string.Empty;
            OracleConnection cn = ConfigTool.GetConnection();
            strSQL = "SELECT * FROM Equ_Rel WHERE ID = " + lngID.ToString();
            DataTable dt = OracleDbHelper.ExecuteDataTable(cn, CommandType.Text, strSQL);
            ConfigTool.CloseConnection(cn);
            foreach (DataRow dr in dt.Rows)
            {
                ee.ID = Decimal.Parse(dr["ID"].ToString());
                ee.Equ_ID = Decimal.Parse(dr["Equ_ID"].ToString());
                ee.RelID = Decimal.Parse(dr["RelID"].ToString());
                ee.RelDescription = dr["RelDescription"].ToString();
            }
            return ee;
        }
        #endregion

        #region GetDataTable
        /// <summary>
        /// 获取关联信息
        /// </summary>
        /// <param name="sWhere"></param>
        /// <param name="sOrder"></param>
        /// <returns>DataTable</returns>
        public DataTable GetDataTableNew(string sWhere, string sOrder,
            String strRelKey)
        {
            string strSQL = string.Empty;
            OracleConnection cn = ConfigTool.GetConnection();
            strSQL = @"select C.*,A.Name,A.Code,A.CostomName,A.partBankName,A.partBranchName
                        from
                        Equ_Desk A,
                        (SELECT * FROM Equ_Rel B
                        WHERE Id in
                                (SELECT ID FROM Equ_Rel
                                 WHERE B.Equ_ID = Equ_ID 
	                         and B.RELID=RELID AND ROWNUM<=1 AND RelKey = {0}
                                )
                        ) C
                        where A.ID=C.RELID and A.deleted=0 ";
            strSQL = String.Format(strSQL, StringTool.SqlQ(strRelKey.ToLower()));
            strSQL += sWhere;
            strSQL += sOrder;
            DataTable dt = OracleDbHelper.ExecuteDataTable(cn, CommandType.Text, strSQL);
            ConfigTool.CloseConnection(cn);
            return dt;
        }

        /// <summary>
        /// 获取关联信息
        /// </summary>
        /// <param name="sWhere"></param>
        /// <param name="sOrder"></param>
        /// <returns>DataTable</returns>
        public DataTable GetDataTableNewWithNoRelkey(string sWhere, string sOrder)
        {
            string strSQL = string.Empty;
            OracleConnection cn = ConfigTool.GetConnection();
            strSQL = @"select C.*,A.Name,A.Code,A.CostomName,A.partBankName,A.partBranchName
                        from
                        Equ_Desk A,
                        (SELECT * FROM Equ_Rel B
                        WHERE Id in
                                (SELECT ID FROM Equ_Rel
                                 WHERE B.Equ_ID = Equ_ID 
	                         and B.RELKEY=RELKEY  AND B.RELID = RELID and ROWNUM <= 1
                                )
                        ) C
                        where A.ID=C.RELID and A.deleted=0 ";
            //strSQL = String.Format(strSQL, StringTool.SqlQ(strRelKey.ToLower()));
            strSQL += sWhere;
            strSQL += sOrder;
            DataTable dt = OracleDbHelper.ExecuteDataTable(cn, CommandType.Text, strSQL);
            ConfigTool.CloseConnection(cn);
            return dt;
        }
        /// <summary>
        /// 获取关联信息
        /// </summary>
        /// <param name="sWhere"></param>
        /// <param name="sOrder"></param>
        /// <returns>DataTable</returns>
        public DataTable GetDataTable(string sWhere, string sOrder)
        {
            string strSQL = string.Empty;
            OracleConnection cn = ConfigTool.GetConnection();
            strSQL = @"select C.*,A.Name,A.Code,A.CostomName,A.partBankName,A.partBranchName
                        from
                        Equ_Desk A,
                        (SELECT * FROM Equ_Rel B
                        WHERE Id in
                                (SELECT ID FROM Equ_Rel
                                 WHERE B.Equ_ID = Equ_ID 
	                         and B.RELID=RELID AND ROWNUM<=1
                                )
                        ) C
                        where A.ID=C.RELID and A.deleted=0 ";
            strSQL += sWhere;
            strSQL += sOrder;
            DataTable dt = OracleDbHelper.ExecuteDataTable(cn, CommandType.Text, strSQL);
            ConfigTool.CloseConnection(cn);
            return dt;
        }
        #endregion

        #region GetDataTable【批量】
        /// <summary>
        /// 
        /// </summary>
        /// <param name="sEquIDs">要批量关联的资产ID串</param>
        /// <param name="sOrder"></param>
        /// <returns></returns>
        public DataTable GetDataTable2(string sEquIDs, string sOrder)
        {
            sEquIDs = sEquIDs + ",";
            string strSQL = string.Empty;
            OracleConnection cn = ConfigTool.GetConnection();
            strSQL = @"declare @EquID varchar(100)
                        set @EquID = ''
                        select @EquID = @EquID + TO_CHAR(A.Equ_ID) +',' from Equ_Rel A,Equ_Rel B
	                        where A.RelID = B.RelID and A.RelPropID = B.RelPropID  and A.RelDescriptionID = B.RelDescriptionID 
	                             and A.Equ_ID <> B.Equ_ID
	                        group by A.Equ_ID
                        select AA.RelID,AA.RelPropID,AA.RelPropName,AA.RelDescriptionID,AA.RelDescription,BB.Name
                        from 
                        (
                        select @EquID as EquIDs,A.* from Equ_Rel A,Equ_Rel B
	                        where A.RelID = B.RelID and A.RelPropID = B.RelPropID and A.RelDescriptionID = B.RelDescriptionID 
	                             and A.Equ_ID <> B.Equ_ID
                        ) AA,Equ_Desk BB where AA.RelID = BB.ID and AA.EquIDs like " + StringTool.SqlQ("%" + sEquIDs + "%") + @"
                        group by AA.RelID,AA.RelPropID,AA.RelPropName,AA.RelDescriptionID,AA.RelDescription,BB.Name";

            strSQL += sOrder;
            DataTable dt = OracleDbHelper.ExecuteDataTable(cn, CommandType.Text, strSQL);
            ConfigTool.CloseConnection(cn);
            return dt;
        }
        #endregion

        #region GetDataTableRel
        /// <summary>
        /// 获取资产关联于信息
        /// </summary>
        /// <param name="sWhere"></param>
        /// <param name="sOrder"></param>
        /// <returns>DataTable</returns>
        public DataTable GetDataTableRel(string sWhere, string sOrder)
        {
            string strSQL = string.Empty;
            OracleConnection cn = ConfigTool.GetConnection();
            strSQL = @"select C.*,A.Name,A.Code,A.CostomName,A.partBankName,A.partBranchName
                        from
                        Equ_Desk A,
                        (SELECT * FROM Equ_Rel B
                        WHERE Id in
                                (SELECT ID FROM Equ_Rel
                                 WHERE B.Equ_ID = Equ_ID 
	                         and B.RELID=RELID AND ROWNUM<=1
                                )
                        ) C
                        where A.ID=C.Equ_ID";
            strSQL += sWhere;
            strSQL += sOrder;
            DataTable dt = OracleDbHelper.ExecuteDataTable(cn, CommandType.Text, strSQL);
            ConfigTool.CloseConnection(cn);
            return dt;
        }
        #endregion

        #region GetDataTableRel
        /// <summary>
        /// 获取资产关联于信息【用于关联描述】
        /// </summary>
        /// <param name="sWhere"></param>
        /// <param name="sOrder"></param>
        /// <returns>DataTable</returns>
        public DataTable GetDataTableRel_Decs(string sWhere, string sOrder)
        {
            string strSQL = string.Empty;
            OracleConnection cn = ConfigTool.GetConnection();
            strSQL = @"select C.*,A.Name,A.Code,A.CostomName,A.partBankName,A.partBranchName
                        from
                        Equ_Desk A,
                        (SELECT * FROM Equ_Rel B
                        WHERE Id in
                                (SELECT  ID FROM Equ_Rel
                                 WHERE B.Equ_ID = Equ_ID 
	                         and B.RELID=RELID
                                )
                        ) C
                        where A.ID=C.Equ_ID";
            strSQL += sWhere;
            strSQL += sOrder;
            DataTable dt = OracleDbHelper.ExecuteDataTable(cn, CommandType.Text, strSQL);
            ConfigTool.CloseConnection(cn);
            return dt;
        }
        #endregion

        #region GetDataTableRel
        /// <summary>
        /// 获取资产关联于信息【用于关联描述2】【子节点资产关系中，只需要描述资产与资产之间的影响关系】
        /// </summary>
        /// <param name="sWhere"></param>
        /// <param name="sOrder"></param>
        /// <returns>DataTable</returns>
        public DataTable GetDataTableRel_Decs2(string sWhere, string sOrder)
        {
            string strSQL = string.Empty;
            OracleConnection cn = ConfigTool.GetConnection();
            strSQL = @"select C.*,A.Name,A.Code,A.CostomName,A.partBankName,A.partBranchName
                        from
                        Equ_Desk A,
                        (SELECT distinct(Equ_ID),RelID FROM Equ_Rel B
                        WHERE Equ_ID in
                                (SELECT distinct(Equ_ID) FROM Equ_Rel
                                 WHERE B.Equ_ID = Equ_ID 
	                         and B.RELID=RELID
                                )
                        ) C
                        where A.ID=C.Equ_ID";
            strSQL += sWhere;
            strSQL += sOrder;
            DataTable dt = OracleDbHelper.ExecuteDataTable(cn, CommandType.Text, strSQL);
            ConfigTool.CloseConnection(cn);
            return dt;
        }
        #endregion

        #region 根据where条件得到资产
        /// <summary>
        /// 根据where条件得到资产
        /// </summary>
        /// <param name="sWhere"></param>
        /// <param name="sOrder"></param>
        /// <returns>DataTable</returns>
        public DataTable GetPropsByID(string sWhere)
        {
            string strSQL = string.Empty;
            OracleConnection cn = ConfigTool.GetConnection();
            strSQL = "SELECT * FROM Equ_Rel Where 1=1";
            strSQL += sWhere;
            DataTable dt = OracleDbHelper.ExecuteDataTable(cn, CommandType.Text, strSQL);
            ConfigTool.CloseConnection(cn);
            return dt;
        }
        #endregion

        #region 根据资产ID和关联资产ID得到关联属性Name串
        /// <summary>
        /// 根据资产ID和关联资产ID得到关联属性Name串
        /// </summary>
        /// <param name="sWhere"></param>
        /// <param name="sOrder"></param>
        /// <returns>DataTable</returns>
        public string GetPropNamesByID(string strID, string strRelID, string strEquRelKey)
        {
            string strEquRelName = string.Empty;
            string strSQL = string.Empty;
            OracleConnection cn = ConfigTool.GetConnection();
            strSQL = @"select * from Equ_Rel
                               where Equ_ID =(SELECT Equ_ID FROM Equ_Rel 
            Where ID =" + strID + " and RelID = " + strRelID + " AND relkey = " 
                        + StringTool.SqlQ(strEquRelKey)
                        + ") and RelID = " + strRelID + " and relkey = " + StringTool.SqlQ(strEquRelKey);
            DataTable dt = OracleDbHelper.ExecuteDataTable(cn, CommandType.Text, strSQL);

            if (dt != null && dt.Rows.Count > 0)
            {
                foreach (DataRow row in dt.Rows)
                {
                    strEquRelName += row["RelPropName"].ToString();
                    strEquRelName += ",";
                }
            }
            if (strEquRelName.Length > 0)
                strEquRelName = strEquRelName.Substring(0, strEquRelName.Length - 1);

            ConfigTool.CloseConnection(cn);
            return strEquRelName;
        }
        #endregion

        #region 递归获取关联资产【描述】
        /// <summary>
        /// 递归获取关联资产【描述】
        /// </summary>
        /// <param name="strRet"></param>
        /// <param name="EquID"></param>
        /// <param name="type"></param>
        /// <param name="sList"></param>
        /// <param name="strTemp"></param>
        /// <param name="iLev">标记影响资产的层数</param>
        public void GetImpactAnalysisChart(ref StringBuilder strRet, string EquID, string type, ref string sList, ref string strTemp, ref int iLev)
        {
            #region 影响资产
            if (type == "4")
            {
                Equ_RelDP ee = new Equ_RelDP();
                //得到第一次传入的关联资产ID
                if (sList == "")
                {
                    strTemp = EquID == string.Empty ? "-1" : EquID;
                }

                int iflg = 0;           //标识是否是同一次循环
                int iCount = 0;         //标识是否为循环的第一条记录
                string sID = EquID == string.Empty ? "-1" : EquID;
                //根据资产ID得到资产名称
                string strEquName = Equ_DeskDP.GetEquNameByID(sID);
                string sWhere = " and RelID=" + sID;
                DataTable dt2 = new DataTable();
                if (iLev == 0)
                {
                    dt2 = ee.GetDataTableRel_Decs(sWhere, string.Empty);
                }
                else
                {
                    dt2 = ee.GetDataTableRel_Decs2(sWhere, string.Empty);
                }

                if (dt2 != null && dt2.Rows.Count > 0)
                {
                    iflg = 0;
                    iLev++;                                                             //第一次进来，表明是第一级影响的资产                    

                    foreach (DataRow dr in dt2.Rows)
                    {
                        if (iLev == 1)
                        {
                            if (iflg == 0)
                            {
                                //第一层
                                if (sList.IndexOf(dr["RelID"].ToString() + ",") < 0 && strTemp.IndexOf(dr["Equ_ID"].ToString()) < 0)
                                {
                                    iCount++;
                                    sList += dr["RelID"].ToString() + ",";                   //存放所有用来作为根的关联资产ID                                

                                    for (int j = 0; j < iLev - 1; j++)
                                    {
                                        strRet.Append("&nbsp;&nbsp;&nbsp;");
                                    }

                                    if (dr["RelPropName"].ToString() == "")
                                    {
                                        //如果属性为空，则为资产影响资产
                                        strRet.Append("您对[" + strEquName + "] 的更改可能会影响资产：<span style='color:Red'><a style='cursor:pointer;' onclick='openwin(" + dr["Equ_ID"].ToString() + ")'>" + dr["Name"].ToString() + "</a></span><br/>");
                                    }
                                    else
                                    {
                                        //如果资产属性不为空，则为资产属性影响资产
                                        strRet.Append("您对[" + strEquName + "] 的 [" + dr["RelPropName"].ToString() + "] 属性的更改可能会影响资产：<span style='color:Red'><a style='cursor:pointer;' onclick='openwin(" + dr["Equ_ID"].ToString() + ")'>" + dr["Name"].ToString() + "</a></span><br/>");
                                    }
                                    GetImpactAnalysisChart2(ref strRet, dr["Equ_ID"].ToString(), type, ref sList, ref strTemp, ref iLev);
                                }
                            }
                            else
                            {
                                if (iCount == 0)
                                {
                                    //如果是同一次循环，且第一次进入；则说明实际中的第一次没能进入上面的循环【已经循环过了，所以这次也不应该进入循环】
                                    break;
                                }
                                else
                                {
                                    iCount++;

                                    for (int j = 0; j < iLev - 1; j++)
                                    {
                                        strRet.Append("&nbsp;&nbsp;&nbsp;");
                                    }
                                    if (dr["RelPropName"].ToString() == "")
                                    {
                                        //如果属性为空，则为资产影响资产
                                        strRet.Append("您对[" + strEquName + "] 的更改可能会影响资产：<span style='color:Red'><a style='cursor:pointer;' onclick='openwin(" + dr["Equ_ID"].ToString() + ")'>" + dr["Name"].ToString() + "</a></span><br/>");
                                    }
                                    else
                                    {
                                        //如果资产属性不为空，则为资产属性影响资产
                                        strRet.Append("您对[" + strEquName + "] 的 [" + dr["RelPropName"].ToString() + "] 属性的更改可能会影响资产：<span style='color:Red'><a style='cursor:pointer;' onclick='openwin(" + dr["Equ_ID"].ToString() + ")'>" + dr["Name"].ToString() + "</a></span><br/>");
                                    }
                                    GetImpactAnalysisChart2(ref strRet, dr["Equ_ID"].ToString(), type, ref sList, ref strTemp, ref iLev);
                                }
                            }
                        }
                        else
                        {
                            if (iflg == 0)
                            {
                                if (sList.IndexOf(dr["RelID"].ToString() + ",") < 0)
                                {
                                    iCount++;

                                    sList += dr["RelID"].ToString() + ",";                   //存放所有用来作为根的关联资产ID

                                    for (int j = 0; j < iLev - 1; j++)
                                    {
                                        strRet.Append("&nbsp;&nbsp;&nbsp;");
                                    }

                                    strRet.Append("[" + strEquName + "] 会影响资产：<span style='color:Red'><a style='cursor:pointer;' onclick='openwin(" + dr["Equ_ID"].ToString() + ")'>" + dr["Name"].ToString() + "</a></span><br/>");

                                    GetImpactAnalysisChart2(ref strRet, dr["Equ_ID"].ToString(), type, ref sList, ref strTemp, ref iLev);
                                }
                            }
                            else
                            {
                                if (iCount == 0)
                                {
                                    //如果是同一次循环，且第一次进入；则说明实际中的第一次没能进入上面的循环【已经循环过了，所以这次也不应该进入循环】
                                    break;
                                }

                                iCount++;

                                for (int j = 0; j < iLev - 1; j++)
                                {
                                    strRet.Append("&nbsp;&nbsp;&nbsp;");
                                }

                                strRet.Append("[" + strEquName + "] 会影响资产：<span style='color:Red'><a style='cursor:pointer;' onclick='openwin(" + dr["Equ_ID"].ToString() + ")'>" + dr["Name"].ToString() + "</a></span><br/>");

                                GetImpactAnalysisChart2(ref strRet, dr["Equ_ID"].ToString(), type, ref sList, ref strTemp, ref iLev);
                            }
                        }

                        iflg = 1;
                    }
                }

                if (iflg == 1)
                {
                    iLev--;
                }
            }
            #endregion
        }
        #endregion

        #region 递归获取关联资产【图】GetImpactAnalysisChart2
        /// <summary>
        /// 递归获取关联资产【图】GetImpactAnalysisChart2
        /// </summary>
        /// <param name="strRet"></param>
        /// <param name="EquID"></param>
        /// <param name="type"></param>
        /// <param name="sList"></param>
        /// <param name="strTemp"></param>
        /// <param name="iLev">标记影响资产的层数</param>
        public void GetImpactAnalysisChart2(ref StringBuilder strRet, string EquID, string type, ref string sList, ref string strTemp, ref int iLev)
        {
            #region 影响资产
            if (type == "4")
            {
                Equ_RelDP ee = new Equ_RelDP();
                //得到第一次传入的关联资产ID
                if (sList == "")
                {
                    strTemp = EquID == string.Empty ? "-1" : EquID;
                }

                int iflg = 0;           //标识是否是同一次循环
                int iCount = 0;         //标识是否是第一次循环
                string sID = EquID == string.Empty ? "-1" : EquID;
                //根据资产ID得到资产名称
                string strEquName = Equ_DeskDP.GetEquNameByID(sID);
                string sWhere = " and RelID=" + sID;
                DataTable dt2 = new DataTable();

                if (iLev == 0)
                {
                    dt2 = ee.GetDataTableRel_Decs(sWhere, string.Empty);
                }
                else
                {
                    dt2 = ee.GetDataTableRel_Decs2(sWhere, string.Empty);
                }

                if (dt2 != null && dt2.Rows.Count > 0)
                {
                    iflg = 0;
                    iLev++;                                                             //第一次进来，表明是第一级影响的资产                    

                    foreach (DataRow dr in dt2.Rows)
                    {
                        //判断是否第二次循环，如果是，则只描述资产影响资产，只循环一次；否则，可以循环多次
                        if (iLev == 1)
                        {

                            if (iflg == 0)
                            {
                                if (sList.IndexOf(dr["RelID"].ToString() + ",") < 0 && strTemp.IndexOf(dr["Equ_ID"].ToString()) < 0)
                                {
                                    iCount++;
                                    sList += dr["RelID"].ToString() + ",";                   //存放所有用来作为根的关联资产ID

                                    for (int j = 0; j < iLev - 1; j++)
                                    {
                                        strRet.Append("&nbsp;&nbsp;&nbsp;");
                                    }
                                    if (dr["RelPropName"].ToString() == "")
                                    {
                                        //如果属性为空，则为资产影响资产
                                        strRet.Append("您对[" + strEquName + "] 的更改可能会影响资产：<span style='color:Red'><a style='cursor:pointer;' onclick='openwin(" + dr["Equ_ID"].ToString() + ")'>" + dr["Name"].ToString() + "</a></span><br/>");
                                    }
                                    else
                                    {
                                        //如果资产属性不为空，则为资产属性影响资产
                                        strRet.Append("您对[" + strEquName + "] 的 [" + dr["RelPropName"].ToString() + "] 属性的更改可能会影响资产：<span style='color:Red'><a style='cursor:pointer;' onclick='openwin(" + dr["Equ_ID"].ToString() + ")'>" + dr["Name"].ToString() + "</a></span><br/>");
                                    }
                                    GetImpactAnalysisChart(ref strRet, dr["Equ_ID"].ToString(), type, ref sList, ref strTemp, ref iLev);
                                }
                            }
                            else
                            {
                                if (iCount == 0)
                                {
                                    //如果是同一次循环，且第一次进入；则说明实际中的第一次没能进入上面的循环【已经循环过了，所以这次也不应该进入循环】
                                    break;
                                }
                                else
                                {
                                    iCount++;

                                    for (int j = 0; j < iLev - 1; j++)
                                    {
                                        strRet.Append("&nbsp;&nbsp;&nbsp;");
                                    }
                                    if (dr["RelPropName"].ToString() == "")
                                    {
                                        //如果属性为空，则为资产影响资产
                                        strRet.Append("您对[" + strEquName + "] 的更改可能会影响资产：<span style='color:Red'><a style='cursor:pointer;' onclick='openwin(" + dr["Equ_ID"].ToString() + ")'>" + dr["Name"].ToString() + "</a></span><br/>");
                                    }
                                    else
                                    {
                                        //如果资产属性不为空，则为资产属性影响资产
                                        strRet.Append("您对[" + strEquName + "] 的 [" + dr["RelPropName"].ToString() + "] 属性的更改可能会影响资产：<span style='color:Red'><a style='cursor:pointer;' onclick='openwin(" + dr["Equ_ID"].ToString() + ")'>" + dr["Name"].ToString() + "</a></span><br/>");
                                    }
                                    GetImpactAnalysisChart(ref strRet, dr["Equ_ID"].ToString(), type, ref sList, ref strTemp, ref iLev);
                                }
                            }
                        }
                        else
                        {
                            if (iflg == 0)
                            {
                                if (sList.IndexOf(dr["RelID"].ToString() + ",") < 0)
                                {
                                    iCount++;

                                    sList += dr["RelID"].ToString() + ",";                   //存放所有用来作为根的关联资产ID

                                    for (int j = 0; j < iLev - 1; j++)
                                    {
                                        strRet.Append("&nbsp;&nbsp;&nbsp;");
                                    }

                                    strRet.Append("[" + strEquName + "] 会影响资产：<span style='color:Red'><a style='cursor:pointer;' onclick='openwin(" + dr["Equ_ID"].ToString() + ")'>" + dr["Name"].ToString() + "</a></span><br/>");

                                    GetImpactAnalysisChart(ref strRet, dr["Equ_ID"].ToString(), type, ref sList, ref strTemp, ref iLev);
                                }
                            }
                            else
                            {
                                if (iCount == 0)
                                {
                                    //如果是同一次循环，且第一次进入；则说明实际中的第一次没能进入上面的循环【已经循环过了，所以这次也不应该进入循环】
                                    break;
                                }

                                iCount++;

                                for (int j = 0; j < iLev - 1; j++)
                                {
                                    strRet.Append("&nbsp;&nbsp;&nbsp;");
                                }

                                strRet.Append("[" + strEquName + "] 会影响资产：<span style='color:Red;'><a style='cursor:pointer;' onclick='openwin(" + dr["Equ_ID"].ToString() + ")'>" + dr["Name"].ToString() + "</a></span><br/>");

                                GetImpactAnalysisChart(ref strRet, dr["Equ_ID"].ToString(), type, ref sList, ref strTemp, ref iLev);
                            }

                        }

                        iflg = 1;
                    }
                }

                if (iflg == 1)
                {
                    iLev--;
                }
            }
            #endregion
        }
        #endregion

        #region 递归获取关联资产
        /// <summary>
        /// 递归获取关联资产
        /// </summary>
        /// <param name="dtRet"></param>
        /// <param name="EquID"></param>
        /// <param name="type"></param>
        /// <returns></returns>
        public DataTable GetImpactAnalysis(ref DataTable dtRet, string EquID, string type, ref string sList, ref string strTemp)
        {
            #region 被影响资产

            if (type == "5")
            {

            }
            #endregion

            #region 影响资产
            else if (type == "4")
            {
                Equ_RelDP ee = new Equ_RelDP();
                //得到第一次传入的关联资产ID
                if (sList == "")
                {
                    strTemp = EquID == string.Empty ? "-1" : EquID;
                }

                int iflg = 0;           //标识是否是同一次循环
                int iCount = 0;         //标识是否是第一次循环
                string sID = EquID == string.Empty ? "-1" : EquID;
                string sWhere = " and RelID=" + sID;
                DataTable dt2 = ee.GetDataTableRel(sWhere, string.Empty);
                if (dt2 != null && dt2.Rows.Count > 0)
                {
                    iflg = 0;

                    foreach (DataRow dr in dt2.Rows)
                    {
                        if (iflg == 0)
                        {
                            if (sList.IndexOf(dr["RelID"].ToString() + ",") < 0 && strTemp.IndexOf(dr["Equ_ID"].ToString()) < 0)
                            {
                                iCount++;
                                sList += dr["RelID"].ToString() + ",";                   //存放所有用来作为根的关联资产ID

                                DataRow row = dtRet.NewRow();
                                row["Equ_ID"] = dr["Equ_ID"].ToString();
                                row["RelID"] = dr["RelID"].ToString();
                                row["NAME"] = dr["Name"].ToString();
                                row["Code"] = dr["Code"].ToString();
                                row["CostomName"] = dr["CostomName"].ToString();
                                row["partBankName"] = dr["partBankName"].ToString();
                                row["partBranchName"] = dr["partBranchName"].ToString();
                                dtRet.Rows.Add(row);
                                GetImpactAnalysis2(ref dtRet, dr["Equ_ID"].ToString(), type, ref sList, ref strTemp);
                            }
                        }
                        else
                        {
                            if (iCount == 0)
                            {
                                //如果是同一次循环，且第一次进入；则说明实际中的第一次没能进入上面的循环【已经循环过了，所以这次也不应该进入循环】
                                break;
                            }
                            else
                            {
                                iCount++;
                                DataRow row = dtRet.NewRow();
                                row["Equ_ID"] = dr["Equ_ID"].ToString();
                                row["RelID"] = dr["RelID"].ToString();
                                row["NAME"] = dr["Name"].ToString();
                                row["Code"] = dr["Code"].ToString();
                                row["CostomName"] = dr["CostomName"].ToString();
                                row["partBankName"] = dr["partBankName"].ToString();
                                row["partBranchName"] = dr["partBranchName"].ToString();
                                dtRet.Rows.Add(row);
                                GetImpactAnalysis2(ref dtRet, dr["Equ_ID"].ToString(), type, ref sList, ref strTemp);
                            }

                        }

                        iflg = 1;
                    }
                }
            }

            return dtRet;

            #endregion
        }
        #endregion

        #region 遍历子资产
        /// <summary>
        /// 遍历子资产
        /// </summary>
        /// <param name="dtRet"></param>
        /// <param name="EquID"></param>
        /// <param name="type"></param>
        /// <param name="sList"></param>
        /// <param name="strTemp"></param>
        protected void GetImpactAnalysis2(ref DataTable dtRet, string EquID, string type, ref string sList, ref string strTemp)
        {
            if (type == "4")
            {
                Equ_RelDP ee = new Equ_RelDP();
                //得到第一次传入的关联资产ID
                if (sList == "")
                {
                    strTemp = EquID == string.Empty ? "-1" : EquID;
                }

                int iflg = 0;           //标识是否是同一次循环
                int iCount = 0;         //标识是否是第一次循环
                string sID = EquID == string.Empty ? "-1" : EquID;
                string sWhere = " and RelID=" + sID;
                DataTable dt2 = ee.GetDataTableRel(sWhere, string.Empty);
                if (dt2 != null && dt2.Rows.Count > 0)
                {
                    iflg = 0;
                    foreach (DataRow dr in dt2.Rows)
                    {
                        if (iflg == 0)
                        {
                            if (sList.IndexOf(dr["RelID"].ToString() + ",") < 0 && strTemp.IndexOf(dr["Equ_ID"].ToString()) < 0)
                            {
                                iCount++;
                                sList += dr["RelID"].ToString() + ",";                   //存放所有用来作为根的关联资产ID

                                DataRow row = dtRet.NewRow();
                                row["Equ_ID"] = dr["Equ_ID"].ToString();
                                row["RelID"] = dr["RelID"].ToString();
                                row["NAME"] = dr["Name"].ToString();
                                row["Code"] = dr["Code"].ToString();
                                row["CostomName"] = dr["CostomName"].ToString();
                                row["partBankName"] = dr["partBankName"].ToString();
                                row["partBranchName"] = dr["partBranchName"].ToString();
                                dtRet.Rows.Add(row);
                                GetImpactAnalysis(ref dtRet, dr["Equ_ID"].ToString(), type, ref sList, ref strTemp);
                            }
                        }
                        else
                        {
                            if (iCount == 0)
                            {
                                //如果是同一次循环，且第一次进入；则说明实际中的第一次没能进入上面的循环【已经循环过了，所以这次也不应该进入循环】
                                break;
                            }
                            else
                            {
                                iCount++;
                                DataRow row = dtRet.NewRow();
                                row["Equ_ID"] = dr["Equ_ID"].ToString();
                                row["RelID"] = dr["RelID"].ToString();
                                row["NAME"] = dr["Name"].ToString();
                                row["Code"] = dr["Code"].ToString();
                                row["CostomName"] = dr["CostomName"].ToString();
                                row["partBankName"] = dr["partBankName"].ToString();
                                row["partBranchName"] = dr["partBranchName"].ToString();
                                dtRet.Rows.Add(row);
                                GetImpactAnalysis(ref dtRet, dr["Equ_ID"].ToString(), type, ref sList, ref strTemp);
                            }
                        }

                        iflg = 1;
                    }
                }
            }
        }
        #endregion

        #region GetEquAllRelXml 获取全部关联关系的XML串

        /// <summary>
        /// 获取全部关联关系的XML串
        /// </summary>
        /// <param name="lngID"></param>
        /// <param name="iBeginX"></param>
        /// <param name="iBeginY"></param>
        /// <param name="iWidth"></param>
        /// <param name="iHeight"></param>
        /// <returns></returns>
        public string GetEquAllRelXml(long lngID, int iBeginX, int iBeginY, int iWidth, int iHeight, string strType)
        {



            string strSQL = string.Empty;
            int iLay = 0;  //递归层数
            int iIndex = 0;  //当前循环的位置
            int iTotlIndex = 0; //总位置

            int iWStep = 3800;
            int iHStep = 1500;

            string sList = "";

            int iTotalStep = 0;
            int iPreTotalStep = 0;
            int iPrePreTotalStep = 0;
            int iPreTotalStepTemp = 1;

            int iTmpBeginX = iBeginX;
            int iTmpBeginY = iBeginY;



            XmlDocument xmlDoc = new XmlDocument();
            xmlDoc.LoadXml("<EQURELATION></EQURELATION>");

            OracleConnection cn = ConfigTool.GetConnection();
            //   ***   暂时 图片URL 固定的，  还没有做完 从设备类型中去取 ****
            if (strType == "4")
            {
                strSQL = @"select B.*,A.Name,A.name as equname,c.imageurl
                        from
                        Equ_Desk A,
                        (SELECT * FROM Equ_Rel BB
                        WHERE Id in
                                (SELECT ID FROM Equ_Rel
                                 WHERE BB.Equ_ID = Equ_ID 
	                         and BB.RELID=RELID AND ROWNUM<=1
                                )
                        ) B,equ_category c
                        where A.ID=B.Equ_ID and A.catalogid = c.catalogid AND A.Deleted = 0";
            }
            else
            {
                strSQL = @"select B.*,A.Name,A.name as equname,c.imageurl
                        from
                        Equ_Desk A,
                        (SELECT * FROM Equ_Rel BB
                        WHERE Id in
                                (SELECT ID FROM Equ_Rel
                                 WHERE BB.Equ_ID = Equ_ID 
	                         and BB.RELID=RELID AND ROWNUM<=1
                                )
                        ) B,equ_category c
                        where A.ID=B.RELID  and A.catalogid = c.catalogid AND A.Deleted = 0";
            }

            DataTable dt = OracleDbHelper.ExecuteDataTable(cn, CommandType.Text, strSQL);
            ConfigTool.CloseConnection(cn);

            DataRow[] rs = new DataRow[10000];
            if (strType == "4")
            {
                rs = dt.Select(" RelID = " + lngID.ToString());
            }
            else
            {
                rs = dt.Select(" equ_id = " + lngID.ToString());
            }

            if (rs.Length > 0)
            {
                //存在相关的关联关系 则填加，开始加第一个节点
                XmlElement xmlEle = xmlDoc.CreateElement("EQU");

                XmlElement xmlRoot = xmlDoc.DocumentElement;

                xmlEle.SetAttribute("INDEX", iTotlIndex.ToString());
                iTotlIndex++;
                xmlEle.SetAttribute("LEFT", iBeginX.ToString());
                xmlEle.SetAttribute("TOP", iBeginY.ToString());
                xmlEle.SetAttribute("EQUID", lngID.ToString());
                xmlEle.SetAttribute("ID", "0");
                xmlEle.SetAttribute("WIDTH", iWidth.ToString());
                xmlEle.SetAttribute("HEIGHT", iHeight.ToString());

                if (GetEquStatus(lngID.ToString()))
                {
                    xmlEle.SetAttribute("DISPLAYERRORICO", "1");
                }
                else
                {
                    xmlEle.SetAttribute("DISPLAYERRORICO", "0");//visible
                }

                Equ_DeskDP equ = new Equ_DeskDP();
                equ = equ.GetReCorded(lngID);

                XmlElement xmlTmp1 = xmlDoc.CreateElement("IMAGESRC");

                string sRootImage = Equ_SubjectDP.GetSubjectImageUrl((long)equ.CatalogID);

                xmlTmp1.InnerText = sRootImage == "" ? "../Images/P_desk.jpg" : sRootImage;

                xmlEle.AppendChild(xmlTmp1);

                xmlTmp1 = xmlDoc.CreateElement("TEXT");
                xmlTmp1.InnerText = equ.Name;
                xmlEle.AppendChild(xmlTmp1);

                xmlRoot.AppendChild(xmlEle);

                if (strType == "4")
                { }
                else
                    sList = lngID.ToString() + ",";

                iLay++;  //增加一层

                int iflg = 0;           //标识是否为同一个循环内
                int iCount = 0;         //标识是否是第一次循环

                foreach (DataRow row in rs)
                {
                    int iTmp = 0;
                    int iTotalStepTmp = 1;
                    //开始添加后续资产
                    xmlEle = xmlDoc.CreateElement("EQU");
                    xmlEle.SetAttribute("INDEX", iTotlIndex.ToString());
                    iTotlIndex++;
                    iTmpBeginX = iBeginX + iWStep;
                    xmlEle.SetAttribute("LEFT", iTmpBeginX.ToString());
                    if (iIndex > 0)
                    {
                        iTmpBeginY = iBeginY + iPreTotalStep * iHStep;
                    }
                    else
                    {
                        iTmpBeginY = iBeginY;
                    }
                    xmlEle.SetAttribute("TOP", iTmpBeginY.ToString());
                    string tempEquId = "";
                    if (strType == "4")
                    {
                        xmlEle.SetAttribute("EQUID", row["Equ_ID"].ToString());
                        tempEquId = row["Equ_ID"].ToString();
                    }
                    else
                    {
                        xmlEle.SetAttribute("EQUID", row["relid"].ToString());
                        tempEquId = row["relid"].ToString();
                    }
                    xmlEle.SetAttribute("ID", row["ID"].ToString());
                    xmlEle.SetAttribute("WIDTH", iWidth.ToString());
                    xmlEle.SetAttribute("HEIGHT", iHeight.ToString());

                    xmlTmp1 = xmlDoc.CreateElement("IMAGESRC");
                    xmlTmp1.InnerText = row["imageurl"].ToString() == "" ? "../Images/EquImage/net.jpg" : row["imageurl"].ToString();

                    xmlEle.AppendChild(xmlTmp1);

                    xmlTmp1 = xmlDoc.CreateElement("TEXT");
                    xmlTmp1.InnerText = row["equname"].ToString();

                    if (GetEquStatus(tempEquId))
                    {
                        xmlEle.SetAttribute("DISPLAYERRORICO", "1");
                    }
                    else
                    {
                        xmlEle.SetAttribute("DISPLAYERRORICO", "0");//visible
                    }
                    xmlEle.AppendChild(xmlTmp1);


                    if (strType == "4")
                    {
                        if (iflg == 0)
                        {
                            if (sList.IndexOf(row["relid"].ToString() + ",") < 0)
                            {
                                iCount++;
                                //未出现过则递归，防止死循环
                                sList += row["relid"].ToString() + ",";

                                //递归加后面的
                                AddNextRelEqu(xmlDoc, dt, long.Parse(row["Equ_ID"].ToString()), iTmpBeginX, iTmpBeginY, iWidth, iHeight, iWStep, iHStep, iLay, ref iTotlIndex, ref iTotalStepTmp, ref xmlRoot, ref sList, strType);

                            }
                        }
                        else
                        {
                            if (iCount == 0)
                            {
                                //如果是同一次循环，且第一次进入；则说明实际中的第一次没能进入上面的循环【已经循环过了，所以这次也不应该进入循环】
                                break;
                            }
                            else
                            {
                                iCount++;
                                AddNextRelEqu(xmlDoc, dt, long.Parse(row["Equ_ID"].ToString()), iTmpBeginX, iTmpBeginY, iWidth, iHeight, iWStep, iHStep, iLay, ref iTotlIndex, ref iTotalStepTmp, ref xmlRoot, ref sList, strType);
                            }
                        }
                    }
                    else
                    {
                        if (sList.IndexOf(row["relid"].ToString() + ",") < 0)
                        {
                            //未出现过则递归，防止死循环
                            sList += row["relid"].ToString() + ",";
                            AddNextRelEqu(xmlDoc, dt, long.Parse(row["relid"].ToString()), iTmpBeginX, iTmpBeginY, iWidth, iHeight, iWStep, iHStep, iLay, ref iTotlIndex, ref iTotalStepTmp, ref xmlRoot, ref sList, strType);
                        }
                    }

                    iflg = 1;               //iflg为1时，表明是本次循环的下一条记录

                    //记录画线前 子循环的步长
                    if (iIndex > 0)
                        iPreTotalStepTemp = iTotalStepTmp;

                    //下一次循环 的第一个环节起点 Y间隔 及这次循环 连线的Y间隔
                    iPrePreTotalStep = iPreTotalStep;
                    iPreTotalStep = iPreTotalStep + iTotalStepTmp;

                    xmlRoot.AppendChild(xmlEle);

                    xmlEle = xmlDoc.CreateElement("LINK");

                    xmlTmp1 = xmlDoc.CreateElement("TEXT");


                    iTmp = iBeginX + iWidth + 200;
                    xmlTmp1.SetAttribute("X", iTmp.ToString());

                    if (iIndex > 0)
                    {
                        iTmp = iBeginY + iPrePreTotalStep * iHStep + iHeight / 2 - 250;
                    }
                    else
                    {
                        iTmp = iBeginY + iHeight / 2 - 250;
                    }

                    xmlTmp1.SetAttribute("Y", iTmp.ToString());

                    xmlTmp1.InnerText = row["reldescription"].ToString();

                    xmlEle.AppendChild(xmlTmp1);

                    #region 下方文字
                    xmlTmp1 = xmlDoc.CreateElement("TEXT2");


                    iTmp = iBeginX + iWidth;
                    xmlTmp1.SetAttribute("X", iTmp.ToString());

                    if (iIndex > 0)
                    {
                        iTmp = iBeginY + iPrePreTotalStep * iHStep + iHeight / 2 + 100;
                    }
                    else
                    {
                        iTmp = iBeginY + iHeight / 2 + 100;
                    }

                    xmlTmp1.SetAttribute("Y", iTmp.ToString());

                    xmlTmp1.InnerText = Equ_DeskDP.GetEquNameByID(row["RelID"].ToString()) + ":[" + GetPropNamesByID(row["ID"].ToString(), row["RelID"].ToString(), row["RelKey"].ToString()) + "]";

                    xmlEle.AppendChild(xmlTmp1);
                    #endregion

                    xmlTmp1 = xmlDoc.CreateElement("_DRAWSTYLE");
                    xmlTmp1.InnerText = "Solid";
                    xmlEle.AppendChild(xmlTmp1);

                    xmlTmp1 = xmlDoc.CreateElement("_ARROWDST");
                    xmlTmp1.InnerText = "Classic";
                    xmlEle.AppendChild(xmlTmp1);

                    xmlTmp1 = xmlDoc.CreateElement("EXTRAPOINTS");

                    // 第一个关系 2个点，第2个关系起 三个点

                    if (iIndex > 0)
                    {
                        //有竖线

                        XmlElement xmlTmp2 = xmlDoc.CreateElement("POINT");
                        iTmp = iBeginX + iWidth;
                        xmlTmp2.SetAttribute("X", iTmp.ToString());
                        if (iIndex == 1)
                        {
                            //第2个时，竖线从起点开始
                            iTmp = iBeginY + iHeight / 2;
                        }
                        else
                        {
                            //iTmp = iBeginY + (iPrePreTotalStep - iPreTotalStepTemp) * iHStep + iHeight / 2;
                            iTmp = iBeginY + iHeight / 2;
                        }
                        xmlTmp2.SetAttribute("Y", iTmp.ToString());

                        xmlTmp1.AppendChild(xmlTmp2);

                        xmlTmp2 = xmlDoc.CreateElement("POINT");
                        iTmp = iBeginX + iWidth;
                        xmlTmp2.SetAttribute("X", iTmp.ToString());
                        iTmp = iBeginY + iPrePreTotalStep * iHStep + iHeight / 2;
                        xmlTmp2.SetAttribute("Y", iTmp.ToString());

                        xmlTmp1.AppendChild(xmlTmp2);

                        //尾节点
                        xmlTmp2 = xmlDoc.CreateElement("POINT");

                        iTmp = iBeginX + iWStep;
                        xmlTmp2.SetAttribute("X", iTmp.ToString());
                        iTmp = iBeginY + iPrePreTotalStep * iHStep + iHeight / 2;
                        xmlTmp2.SetAttribute("Y", iTmp.ToString());

                        xmlTmp1.AppendChild(xmlTmp2);
                    }
                    else
                    {
                        //无竖线
                        XmlElement xmlTmp2 = xmlDoc.CreateElement("POINT");
                        iTmp = iBeginX + iWidth;
                        xmlTmp2.SetAttribute("X", iTmp.ToString());
                        iTmp = iBeginY + iHeight / 2;
                        xmlTmp2.SetAttribute("Y", iTmp.ToString());

                        xmlTmp1.AppendChild(xmlTmp2);


                        //尾节点
                        xmlTmp2 = xmlDoc.CreateElement("POINT");

                        iTmp = iBeginX + iWStep;
                        xmlTmp2.SetAttribute("X", iTmp.ToString());
                        iTmp = iBeginY + iHeight / 2;
                        xmlTmp2.SetAttribute("Y", iTmp.ToString());

                        xmlTmp1.AppendChild(xmlTmp2);

                    }


                    xmlEle.AppendChild(xmlTmp1);

                    xmlRoot.AppendChild(xmlEle);

                    iIndex++;

                }

            }
            else
            {
                //若不存在关联，则增加一个根资产      
                XmlElement xmlRoot = xmlDoc.DocumentElement;

                XmlElement xmlEle = xmlDoc.CreateElement("EQU");

                xmlEle.SetAttribute("INDEX", iTotlIndex.ToString());
                xmlEle.SetAttribute("LEFT", iBeginX.ToString());
                xmlEle.SetAttribute("TOP", iBeginY.ToString());
                xmlEle.SetAttribute("EQUID", lngID.ToString());
                xmlEle.SetAttribute("ID", "0");
                xmlEle.SetAttribute("WIDTH", iWidth.ToString());
                xmlEle.SetAttribute("HEIGHT", iHeight.ToString());

                if (GetEquStatus(lngID.ToString()))
                {
                    xmlEle.SetAttribute("DISPLAYERRORICO", "1");
                }
                else
                {
                    xmlEle.SetAttribute("DISPLAYERRORICO", "0");//visible
                }

                Equ_DeskDP equ = new Equ_DeskDP();
                equ = equ.GetReCorded(lngID);

                XmlElement xmlTmp1 = xmlDoc.CreateElement("IMAGESRC");

                string sRootImage = Equ_SubjectDP.GetSubjectImageUrl((long)equ.CatalogID);

                xmlTmp1.InnerText = sRootImage == "" ? "../Images/P_desk.jpg" : sRootImage;

                xmlEle.AppendChild(xmlTmp1);

                xmlTmp1 = xmlDoc.CreateElement("TEXT");
                xmlTmp1.InnerText = equ.Name;

                xmlEle.AppendChild(xmlTmp1);

                xmlRoot.AppendChild(xmlEle);
            }
            return xmlDoc.InnerXml;

        }


        /// <summary>
        /// 获取全部关联关系的XML串, 按关键词(strSearchKey)过滤. 
        /// </summary>
        /// <param name="lngID"></param>
        /// <param name="iBeginX"></param>
        /// <param name="iBeginY"></param>
        /// <param name="iWidth"></param>
        /// <param name="iHeight"></param>
        /// <returns></returns>
        public string GetEquAllRelXmlByKey(long lngID, int iBeginX,
            int iBeginY, int iWidth, int iHeight, string strType, String strSearchKey)
        {
            string strSQL = string.Empty;
            int iLay = 0;  //递归层数
            int iIndex = 0;  //当前循环的位置
            int iTotlIndex = 0; //总位置

            int iWStep = 3800;
            int iHStep = 1500;

            string sList = "";

            int iTotalStep = 0;
            int iPreTotalStep = 0;
            int iPrePreTotalStep = 0;
            int iPreTotalStepTemp = 1;

            int iTmpBeginX = iBeginX;
            int iTmpBeginY = iBeginY;



            XmlDocument xmlDoc = new XmlDocument();
            xmlDoc.LoadXml("<EQURELATION></EQURELATION>");

            OracleConnection cn = ConfigTool.GetConnection();
            //   ***   暂时 图片URL 固定的，  还没有做完 从设备类型中去取 ****
            if (strType == "4")
            {
                strSQL = @"select B.*,A.Name,A.name as equname,c.imageurl
                        from
                        Equ_Desk A,
                        (SELECT * FROM Equ_Rel BB
                        WHERE Id in
                                (SELECT ID FROM Equ_Rel
                                 WHERE BB.Equ_ID = Equ_ID 
	                         and BB.RELID=RELID AND ROWNUM<=1 AND BB.RelKey = RelKey
                                )
                        ) B,equ_category c
                        where A.ID=B.Equ_ID and A.catalogid = c.catalogid AND A.Deleted = 0";
            }
            else
            {
                strSQL = @"select B.*,A.Name,A.name as equname,c.imageurl
                        from
                        Equ_Desk A,
                        (SELECT * FROM Equ_Rel BB
                        WHERE Id in
                                (SELECT ID FROM Equ_Rel
                                 WHERE BB.Equ_ID = Equ_ID 
	                         and BB.RELID=RELID AND ROWNUM<=1 AND BB.RelKey = RelKey
                                )
                        ) B,equ_category c
                        where A.ID=B.RELID  and A.catalogid = c.catalogid AND A.Deleted = 0";
            }

            DataTable dt = OracleDbHelper.ExecuteDataTable(cn, CommandType.Text, strSQL);
            ConfigTool.CloseConnection(cn);

            DataRow[] rs = new DataRow[10000];
            if (strType == "4")
            {
                rs = dt.Select(" RelID = " + lngID.ToString());
            }
            else
            {
                rs = dt.Select(" equ_id = " + lngID.ToString());
            }

            if (rs.Length > 0)
            {
                //Equ_DeskDP equ_bak = new Equ_DeskDP();
                //equ_bak = equ_bak.GetReCorded(lngID);
                //String strEquName = equ_bak.Name;
                //if (strEquName.Contains("IBM")) return String.Empty;
                //存在相关的关联关系 则填加，开始加第一个节点
                XmlElement xmlEle = xmlDoc.CreateElement("EQU");

                XmlElement xmlRoot = xmlDoc.DocumentElement;

                xmlEle.SetAttribute("INDEX", iTotlIndex.ToString());
                iTotlIndex++;
                xmlEle.SetAttribute("LEFT", iBeginX.ToString());
                xmlEle.SetAttribute("TOP", iBeginY.ToString());
                xmlEle.SetAttribute("EQUID", lngID.ToString());
                xmlEle.SetAttribute("ID", "0");
                xmlEle.SetAttribute("WIDTH", iWidth.ToString());
                xmlEle.SetAttribute("HEIGHT", iHeight.ToString());

                if (GetEquStatus(lngID.ToString()))
                {
                    xmlEle.SetAttribute("DISPLAYERRORICO", "1");
                }
                else
                {
                    xmlEle.SetAttribute("DISPLAYERRORICO", "0");//visible
                }

                Equ_DeskDP equ = new Equ_DeskDP();
                equ = equ.GetReCorded(lngID);

                XmlElement xmlTmp1 = xmlDoc.CreateElement("IMAGESRC");

                string sRootImage = Equ_SubjectDP.GetSubjectImageUrl((long)equ.CatalogID);

                xmlTmp1.InnerText = sRootImage == "" ? "../Images/P_desk.jpg" : sRootImage;

                xmlEle.AppendChild(xmlTmp1);

                xmlTmp1 = xmlDoc.CreateElement("TEXT");
                xmlTmp1.InnerText = equ.Name;
                xmlEle.AppendChild(xmlTmp1);

                xmlRoot.AppendChild(xmlEle);

                if (strType == "4")
                { }
                else
                    sList = lngID.ToString() + ",";

                iLay++;  //增加一层

                int iflg = 0;           //标识是否为同一个循环内
                int iCount = 0;         //标识是否是第一次循环

                foreach (DataRow row in rs)
                {
                    String strEquKey = row["RelKey"].ToString().Trim().ToLower();
                    if (!strEquKey.Contains(strSearchKey.Trim().ToLower())) continue;
                    int iTmp = 0;
                    int iTotalStepTmp = 1;
                    //开始添加后续资产
                    xmlEle = xmlDoc.CreateElement("EQU");
                    xmlEle.SetAttribute("INDEX", iTotlIndex.ToString());
                    iTotlIndex++;
                    iTmpBeginX = iBeginX + iWStep;
                    xmlEle.SetAttribute("LEFT", iTmpBeginX.ToString());
                    if (iIndex > 0)
                    {
                        iTmpBeginY = iBeginY + iPreTotalStep * iHStep;
                    }
                    else
                    {
                        iTmpBeginY = iBeginY;
                    }
                    xmlEle.SetAttribute("TOP", iTmpBeginY.ToString());
                    string tempEquId = "";
                    if (strType == "4")
                    {
                        xmlEle.SetAttribute("EQUID", row["Equ_ID"].ToString());
                        tempEquId = row["Equ_ID"].ToString();
                    }
                    else
                    {
                        xmlEle.SetAttribute("EQUID", row["relid"].ToString());
                        tempEquId = row["relid"].ToString();
                    }
                    xmlEle.SetAttribute("ID", row["ID"].ToString());
                    xmlEle.SetAttribute("WIDTH", iWidth.ToString());
                    xmlEle.SetAttribute("HEIGHT", iHeight.ToString());

                    xmlTmp1 = xmlDoc.CreateElement("IMAGESRC");
                    xmlTmp1.InnerText = row["imageurl"].ToString() == "" ? "../Images/EquImage/net.jpg" : row["imageurl"].ToString();

                    xmlEle.AppendChild(xmlTmp1);

                    xmlTmp1 = xmlDoc.CreateElement("TEXT");
                    xmlTmp1.InnerText = row["equname"].ToString();

                    if (GetEquStatus(tempEquId))
                    {
                        xmlEle.SetAttribute("DISPLAYERRORICO", "1");
                    }
                    else
                    {
                        xmlEle.SetAttribute("DISPLAYERRORICO", "0");//visible
                    }
                    xmlEle.AppendChild(xmlTmp1);


                    if (strType == "4")
                    {
                        if (iflg == 0)
                        {
                            if (sList.IndexOf(row["relid"].ToString() + ",") < 0)
                            {
                                iCount++;
                                //未出现过则递归，防止死循环
                                sList += row["relid"].ToString() + ",";

                                //递归加后面的
                                AddNextRelEquByKey(xmlDoc, dt, long.Parse(row["Equ_ID"].ToString()), iTmpBeginX, iTmpBeginY, iWidth, iHeight, iWStep, iHStep, iLay, ref iTotlIndex, ref iTotalStepTmp, ref xmlRoot, ref sList, strType, strSearchKey);

                            }
                        }
                        else
                        {
                            if (iCount == 0)
                            {
                                //如果是同一次循环，且第一次进入；则说明实际中的第一次没能进入上面的循环【已经循环过了，所以这次也不应该进入循环】
                                break;
                            }
                            else
                            {
                                iCount++;
                                AddNextRelEquByKey(xmlDoc, dt, long.Parse(row["Equ_ID"].ToString()), iTmpBeginX, iTmpBeginY, iWidth, iHeight, iWStep, iHStep, iLay, ref iTotlIndex, ref iTotalStepTmp, ref xmlRoot, ref sList, strType, strSearchKey);
                            }
                        }
                    }
                    else
                    {
                        if (sList.IndexOf(row["relid"].ToString() + ",") < 0)
                        {
                            //未出现过则递归，防止死循环
                            sList += row["relid"].ToString() + ",";
                            AddNextRelEquByKey(xmlDoc, dt, long.Parse(row["relid"].ToString()), iTmpBeginX, iTmpBeginY, iWidth, iHeight, iWStep, iHStep, iLay, ref iTotlIndex, ref iTotalStepTmp, ref xmlRoot, ref sList, strType, strSearchKey);
                        }
                    }

                    iflg = 1;               //iflg为1时，表明是本次循环的下一条记录

                    //记录画线前 子循环的步长
                    if (iIndex > 0)
                        iPreTotalStepTemp = iTotalStepTmp;

                    //下一次循环 的第一个环节起点 Y间隔 及这次循环 连线的Y间隔
                    iPrePreTotalStep = iPreTotalStep;
                    iPreTotalStep = iPreTotalStep + iTotalStepTmp;

                    xmlRoot.AppendChild(xmlEle);

                    xmlEle = xmlDoc.CreateElement("LINK");

                    xmlTmp1 = xmlDoc.CreateElement("TEXT");


                    iTmp = iBeginX + iWidth + 200;
                    xmlTmp1.SetAttribute("X", iTmp.ToString());

                    if (iIndex > 0)
                    {
                        iTmp = iBeginY + iPrePreTotalStep * iHStep + iHeight / 2 - 250;
                    }
                    else
                    {
                        iTmp = iBeginY + iHeight / 2 - 250;
                    }

                    xmlTmp1.SetAttribute("Y", iTmp.ToString());

                    xmlTmp1.InnerText = row["reldescription"].ToString();

                    xmlEle.AppendChild(xmlTmp1);

                    #region 下方文字
                    xmlTmp1 = xmlDoc.CreateElement("TEXT2");


                    iTmp = iBeginX + iWidth;
                    xmlTmp1.SetAttribute("X", iTmp.ToString());

                    if (iIndex > 0)
                    {
                        iTmp = iBeginY + iPrePreTotalStep * iHStep + iHeight / 2 + 100;
                    }
                    else
                    {
                        iTmp = iBeginY + iHeight / 2 + 100;
                    }

                    xmlTmp1.SetAttribute("Y", iTmp.ToString());

                    xmlTmp1.InnerText = Equ_DeskDP.GetEquNameByID(row["RelID"].ToString()) + ":[" + GetPropNamesByID(row["ID"].ToString(), row["RelID"].ToString(), row["RelKey"].ToString()) + "]";

                    xmlEle.AppendChild(xmlTmp1);
                    #endregion

                    xmlTmp1 = xmlDoc.CreateElement("_DRAWSTYLE");
                    xmlTmp1.InnerText = "Solid";
                    xmlEle.AppendChild(xmlTmp1);

                    xmlTmp1 = xmlDoc.CreateElement("_ARROWDST");
                    xmlTmp1.InnerText = "Classic";
                    xmlEle.AppendChild(xmlTmp1);

                    xmlTmp1 = xmlDoc.CreateElement("EXTRAPOINTS");

                    // 第一个关系 2个点，第2个关系起 三个点

                    if (iIndex > 0)
                    {
                        //有竖线

                        XmlElement xmlTmp2 = xmlDoc.CreateElement("POINT");
                        iTmp = iBeginX + iWidth;
                        xmlTmp2.SetAttribute("X", iTmp.ToString());
                        if (iIndex == 1)
                        {
                            //第2个时，竖线从起点开始
                            iTmp = iBeginY + iHeight / 2;
                        }
                        else
                        {
                            //iTmp = iBeginY + (iPrePreTotalStep - iPreTotalStepTemp) * iHStep + iHeight / 2;
                            iTmp = iBeginY + iHeight / 2;
                        }
                        xmlTmp2.SetAttribute("Y", iTmp.ToString());

                        xmlTmp1.AppendChild(xmlTmp2);

                        xmlTmp2 = xmlDoc.CreateElement("POINT");
                        iTmp = iBeginX + iWidth;
                        xmlTmp2.SetAttribute("X", iTmp.ToString());
                        iTmp = iBeginY + iPrePreTotalStep * iHStep + iHeight / 2;
                        xmlTmp2.SetAttribute("Y", iTmp.ToString());

                        xmlTmp1.AppendChild(xmlTmp2);

                        //尾节点
                        xmlTmp2 = xmlDoc.CreateElement("POINT");

                        iTmp = iBeginX + iWStep;
                        xmlTmp2.SetAttribute("X", iTmp.ToString());
                        iTmp = iBeginY + iPrePreTotalStep * iHStep + iHeight / 2;
                        xmlTmp2.SetAttribute("Y", iTmp.ToString());

                        xmlTmp1.AppendChild(xmlTmp2);
                    }
                    else
                    {
                        //无竖线
                        XmlElement xmlTmp2 = xmlDoc.CreateElement("POINT");
                        iTmp = iBeginX + iWidth;
                        xmlTmp2.SetAttribute("X", iTmp.ToString());
                        iTmp = iBeginY + iHeight / 2;
                        xmlTmp2.SetAttribute("Y", iTmp.ToString());

                        xmlTmp1.AppendChild(xmlTmp2);


                        //尾节点
                        xmlTmp2 = xmlDoc.CreateElement("POINT");

                        iTmp = iBeginX + iWStep;
                        xmlTmp2.SetAttribute("X", iTmp.ToString());
                        iTmp = iBeginY + iHeight / 2;
                        xmlTmp2.SetAttribute("Y", iTmp.ToString());

                        xmlTmp1.AppendChild(xmlTmp2);

                    }


                    xmlEle.AppendChild(xmlTmp1);

                    xmlRoot.AppendChild(xmlEle);

                    iIndex++;

                }

            }
            else
            {
                //若不存在关联，则增加一个根资产      
                XmlElement xmlRoot = xmlDoc.DocumentElement;

                XmlElement xmlEle = xmlDoc.CreateElement("EQU");

                xmlEle.SetAttribute("INDEX", iTotlIndex.ToString());
                xmlEle.SetAttribute("LEFT", iBeginX.ToString());
                xmlEle.SetAttribute("TOP", iBeginY.ToString());
                xmlEle.SetAttribute("EQUID", lngID.ToString());
                xmlEle.SetAttribute("ID", "0");
                xmlEle.SetAttribute("WIDTH", iWidth.ToString());
                xmlEle.SetAttribute("HEIGHT", iHeight.ToString());

                if (GetEquStatus(lngID.ToString()))
                {
                    xmlEle.SetAttribute("DISPLAYERRORICO", "1");
                }
                else
                {
                    xmlEle.SetAttribute("DISPLAYERRORICO", "0");//visible
                }

                Equ_DeskDP equ = new Equ_DeskDP();
                equ = equ.GetReCorded(lngID);

                XmlElement xmlTmp1 = xmlDoc.CreateElement("IMAGESRC");

                string sRootImage = Equ_SubjectDP.GetSubjectImageUrl((long)equ.CatalogID);

                xmlTmp1.InnerText = sRootImage == "" ? "../Images/P_desk.jpg" : sRootImage;

                xmlEle.AppendChild(xmlTmp1);

                xmlTmp1 = xmlDoc.CreateElement("TEXT");
                xmlTmp1.InnerText = equ.Name;

                xmlEle.AppendChild(xmlTmp1);

                xmlRoot.AppendChild(xmlEle);
            }
            return xmlDoc.InnerXml;

        }


        public bool GetEquStatus(string equId)
        {
            if (string.IsNullOrEmpty(equId) || (equId == "0"))
            {
                throw new ArgumentNullException("equId is null or zero.(" + equId + ")");
            }
            string sql = string.Format(@"SELECT EquipmentID  FROM CST_ISSUES I WHERE EquipmentID={0} AND 
                            EXISTS (SELECT * FROM ES_FLOW F WHERE F.FLOWID =I.FLOWID AND F.STATUS IN (20,30))", equId);

            OracleConnection cn = ConfigTool.GetConnection("SQLConnString");//获取连接
            try
            {

                int result = Convert.ToInt32(OracleDbHelper.ExecuteScalar(cn, CommandType.Text, sql));//执行查询
                return (result == 0) ? true : false;
            }
            catch
            {
                throw;
            }
            finally
            {

                ConfigTool.CloseConnection(cn);//关闭连接
            }

        }

        /// <summary>
        /// 遍历追加子节点. 根据关键词 (strSearchKey) 过滤.
        /// </summary>
        /// <param name="xmlDoc"></param>
        /// <param name="dt"></param>
        /// <param name="lngID"></param>
        /// <param name="iBeginX"></param>
        /// <param name="iBeginY"></param>
        /// <param name="iWidth"></param>
        /// <param name="iHeight"></param>
        /// <param name="iWStep"></param>
        /// <param name="iHStep"></param>
        /// <param name="iLay"></param>
        /// <param name="iTotlIndex"></param>
        /// <param name="iTotalStep"></param>
        /// <param name="xmlRoot"></param>
        /// <param name="sList"></param>
        /// <param name="strType"></param>
        private void AddNextRelEquByKey(XmlDocument xmlDoc, DataTable dt,
            long lngID, int iBeginX, int iBeginY,
            int iWidth, int iHeight, int iWStep,
            int iHStep, int iLay, ref int iTotlIndex,
            ref int iTotalStep,
            ref XmlElement xmlRoot,
            ref string sList, string strType, String strSearchKey)
        {
            DataRow[] rs = new DataRow[10000];
            if (strType == "4")
            {
                rs = dt.Select(" RelID = " + lngID.ToString());
            }
            else
            {
                rs = dt.Select(" equ_id = " + lngID.ToString());
            }

            int iPreTotalStep = 0;
            int iPrePreTotalStep = 0;
            int iPreTotalStepTemp = 1;


            XmlElement xmlEle;
            XmlElement xmlTmp1;
            int iIndex = 0;  //当前循环的位置

            int iTmpBeginX = iBeginX;
            int iTmpBeginY = iBeginY;

            int iflg = 0;           //标识是否为同一个循环内
            int iCount = 0;         //标识是否为循环的第一条记录

            iLay++;  //增加一层
            foreach (DataRow row in rs)
            {
                String strEquKey = row["RelKey"].ToString().Trim().ToLower();
                if (!strEquKey.Contains(strSearchKey.Trim().ToLower())) continue;
                int iTmp = 0;
                int iTotalStepTmp = 1;
                //开始添加后续资产
                xmlEle = xmlDoc.CreateElement("EQU");
                xmlEle.SetAttribute("INDEX", iTotlIndex.ToString());
                iTotlIndex++;
                iTmpBeginX = iBeginX + iWStep;
                xmlEle.SetAttribute("LEFT", iTmpBeginX.ToString());
                if (iIndex > 0)
                {
                    iTmpBeginY = iBeginY + iPreTotalStep * iHStep;
                }
                else
                {
                    iTmpBeginY = iBeginY;
                }
                xmlEle.SetAttribute("TOP", iTmpBeginY.ToString());
                string equId = "";
                if (strType == "4")
                {
                    xmlEle.SetAttribute("EQUID", row["Equ_ID"].ToString());
                    equId = row["Equ_ID"].ToString();
                }
                else
                {
                    xmlEle.SetAttribute("EQUID", row["relid"].ToString());
                    equId = row["relid"].ToString();
                }


                if (GetEquStatus(equId))
                {
                    xmlEle.SetAttribute("DISPLAYERRORICO", "1");
                }
                else
                {
                    xmlEle.SetAttribute("DISPLAYERRORICO", "0");//visible
                }

                xmlEle.SetAttribute("ID", row["ID"].ToString());
                xmlEle.SetAttribute("WIDTH", iWidth.ToString());
                xmlEle.SetAttribute("HEIGHT", iHeight.ToString());

                xmlTmp1 = xmlDoc.CreateElement("IMAGESRC");
                xmlTmp1.InnerText = row["imageurl"].ToString() == "" ? "../Images/EquImage/net.jpg" : row["imageurl"].ToString();

                xmlEle.AppendChild(xmlTmp1);

                xmlTmp1 = xmlDoc.CreateElement("TEXT");
                xmlTmp1.InnerText = row["equname"].ToString();
                xmlEle.AppendChild(xmlTmp1);



                if (strType == "4")
                {
                    if (iflg == 0)
                    {
                        if (sList.IndexOf(row["relid"].ToString() + ",") < 0)
                        {
                            iCount++;
                            //未出现过则递归，防止死循环
                            sList += row["relid"].ToString() + ",";

                            //递归加后面的
                            AddNextRelEquByKey(xmlDoc, dt, long.Parse(row["Equ_ID"].ToString()), iTmpBeginX, iTmpBeginY, iWidth, iHeight, iWStep, iHStep, iLay, ref iTotlIndex, ref iTotalStepTmp, ref xmlRoot, ref sList, strType, strSearchKey);

                        }
                    }
                    else
                    {
                        if (iCount == 0)
                        {
                            //如果是同一次循环，且第一次进入；则说明实际中的第一次没能进入上面的循环【已经循环过了，所以这次也不应该进入循环】
                            break;
                        }
                        else
                        {
                            iCount++;
                            AddNextRelEquByKey(xmlDoc, dt, long.Parse(row["Equ_ID"].ToString()), iTmpBeginX, iTmpBeginY, iWidth, iHeight, iWStep, iHStep, iLay, ref iTotlIndex, ref iTotalStepTmp, ref xmlRoot, ref sList, strType, strSearchKey);
                        }
                    }
                }
                else
                {
                    if (sList.IndexOf(row["relid"].ToString() + ",") < 0)
                    {
                        //未出现过则递归，防止死循环
                        sList += row["relid"].ToString() + ",";

                        //递归加后面的
                        AddNextRelEquByKey(xmlDoc, dt, long.Parse(row["relid"].ToString()), iTmpBeginX, iTmpBeginY, iWidth, iHeight, iWStep, iHStep, iLay, ref iTotlIndex, ref iTotalStepTmp, ref xmlRoot, ref sList, strType, strSearchKey);

                    }
                }


                //记录画线前 子循环的步长
                if (iIndex > 0)
                    iPreTotalStepTemp = iTotalStepTmp;

                //iTotalStep = iTotalStepTmp;

                ////后面的小过INDEX 时 间隔增1
                //if (iIndex >= iTotalStep)
                //    iTotalStep++;


                //下一次循环 的第一个环节起点 Y间隔 及这次循环 连线的Y间隔
                iPrePreTotalStep = iPreTotalStep;
                iPreTotalStep = iPreTotalStep + iTotalStepTmp;
                iTotalStep = iPreTotalStep;


                xmlRoot.AppendChild(xmlEle);

                xmlEle = xmlDoc.CreateElement("LINK");

                xmlTmp1 = xmlDoc.CreateElement("TEXT");


                iTmp = iBeginX + iWidth + 200;
                xmlTmp1.SetAttribute("X", iTmp.ToString());

                if (iIndex > 0)
                {
                    iTmp = iBeginY + iPrePreTotalStep * iHStep + iHeight / 2 - 250;
                }
                else
                {
                    iTmp = iBeginY + iHeight / 2 - 250;
                }
                xmlTmp1.SetAttribute("Y", iTmp.ToString());

                xmlTmp1.InnerText = row["reldescription"].ToString();

                xmlEle.AppendChild(xmlTmp1);

                #region 下方文字
                xmlTmp1 = xmlDoc.CreateElement("TEXT2");

                iTmp = iBeginX + iWidth;
                xmlTmp1.SetAttribute("X", iTmp.ToString());

                if (iIndex > 0)
                {
                    iTmp = iBeginY + iPrePreTotalStep * iHStep + iHeight / 2 + 100;
                }
                else
                {
                    iTmp = iBeginY + iHeight / 2 + 100;
                }

                xmlTmp1.SetAttribute("Y", iTmp.ToString());

                xmlTmp1.InnerText = Equ_DeskDP.GetEquNameByID(row["RelID"].ToString()) + ":[" + GetPropNamesByID(row["ID"].ToString(), row["RelID"].ToString(), row["RelKey"].ToString()) + "]";

                xmlEle.AppendChild(xmlTmp1);
                #endregion

                xmlTmp1 = xmlDoc.CreateElement("_DRAWSTYLE");
                xmlTmp1.InnerText = "Solid";
                xmlEle.AppendChild(xmlTmp1);

                xmlTmp1 = xmlDoc.CreateElement("_ARROWDST");
                xmlTmp1.InnerText = "Classic";
                xmlEle.AppendChild(xmlTmp1);

                xmlTmp1 = xmlDoc.CreateElement("EXTRAPOINTS");

                // 第一个关系 2个点，第2个关系起 三个点
                if (iIndex > 0)
                {
                    //有竖线
                    XmlElement xmlTmp2 = xmlDoc.CreateElement("POINT");
                    iTmp = iBeginX + iWidth;
                    xmlTmp2.SetAttribute("X", iTmp.ToString());
                    if (iIndex == 1)
                    {
                        //第2个时，竖线从起点开始
                        iTmp = iBeginY + iHeight / 2;
                    }
                    else
                    {
                        //iTmp = iBeginY + (iPrePreTotalStep - iPreTotalStepTemp) * iHStep + iHeight / 2;
                        iTmp = iBeginY + iHeight / 2;
                    }
                    xmlTmp2.SetAttribute("Y", iTmp.ToString());

                    xmlTmp1.AppendChild(xmlTmp2);

                    xmlTmp2 = xmlDoc.CreateElement("POINT");
                    iTmp = iBeginX + iWidth;
                    xmlTmp2.SetAttribute("X", iTmp.ToString());
                    iTmp = iBeginY + iPrePreTotalStep * iHStep + iHeight / 2;
                    xmlTmp2.SetAttribute("Y", iTmp.ToString());

                    xmlTmp1.AppendChild(xmlTmp2);

                    //尾节点
                    xmlTmp2 = xmlDoc.CreateElement("POINT");

                    iTmp = iBeginX + iWStep;
                    xmlTmp2.SetAttribute("X", iTmp.ToString());
                    iTmp = iBeginY + iPrePreTotalStep * iHStep + iHeight / 2;
                    xmlTmp2.SetAttribute("Y", iTmp.ToString());



                    xmlTmp1.AppendChild(xmlTmp2);



                }
                else
                {
                    //无竖线
                    XmlElement xmlTmp2 = xmlDoc.CreateElement("POINT");
                    iTmp = iBeginX + iWidth;
                    xmlTmp2.SetAttribute("X", iTmp.ToString());
                    iTmp = iBeginY + iHeight / 2;
                    xmlTmp2.SetAttribute("Y", iTmp.ToString());

                    xmlTmp1.AppendChild(xmlTmp2);



                    //尾节点
                    xmlTmp2 = xmlDoc.CreateElement("POINT");

                    iTmp = iBeginX + iWStep;
                    xmlTmp2.SetAttribute("X", iTmp.ToString());
                    iTmp = iBeginY + iHeight / 2;
                    xmlTmp2.SetAttribute("Y", iTmp.ToString());

                    xmlTmp1.AppendChild(xmlTmp2);

                }

                xmlEle.AppendChild(xmlTmp1);

                xmlRoot.AppendChild(xmlEle);
                iIndex++;



            }



        }

        private void AddNextRelEqu(XmlDocument xmlDoc, DataTable dt, long lngID, int iBeginX, int iBeginY, int iWidth, int iHeight, int iWStep, int iHStep, int iLay, ref int iTotlIndex, ref int iTotalStep, ref XmlElement xmlRoot, ref string sList, string strType)
        {
            DataRow[] rs = new DataRow[10000];
            if (strType == "4")
            {
                rs = dt.Select(" RelID = " + lngID.ToString());
            }
            else
            {
                rs = dt.Select(" equ_id = " + lngID.ToString());
            }

            int iPreTotalStep = 0;
            int iPrePreTotalStep = 0;
            int iPreTotalStepTemp = 1;


            XmlElement xmlEle;
            XmlElement xmlTmp1;
            int iIndex = 0;  //当前循环的位置

            int iTmpBeginX = iBeginX;
            int iTmpBeginY = iBeginY;

            int iflg = 0;           //标识是否为同一个循环内
            int iCount = 0;         //标识是否为循环的第一条记录

            iLay++;  //增加一层
            foreach (DataRow row in rs)
            {

                int iTmp = 0;
                int iTotalStepTmp = 1;
                //开始添加后续资产
                xmlEle = xmlDoc.CreateElement("EQU");
                xmlEle.SetAttribute("INDEX", iTotlIndex.ToString());
                iTotlIndex++;
                iTmpBeginX = iBeginX + iWStep;
                xmlEle.SetAttribute("LEFT", iTmpBeginX.ToString());
                if (iIndex > 0)
                {
                    iTmpBeginY = iBeginY + iPreTotalStep * iHStep;
                }
                else
                {
                    iTmpBeginY = iBeginY;
                }
                xmlEle.SetAttribute("TOP", iTmpBeginY.ToString());
                string equId = "";
                if (strType == "4")
                {
                    xmlEle.SetAttribute("EQUID", row["Equ_ID"].ToString());
                    equId = row["Equ_ID"].ToString();
                }
                else
                {
                    xmlEle.SetAttribute("EQUID", row["relid"].ToString());
                    equId = row["relid"].ToString();
                }


                if (GetEquStatus(equId))
                {
                    xmlEle.SetAttribute("DISPLAYERRORICO", "1");
                }
                else
                {
                    xmlEle.SetAttribute("DISPLAYERRORICO", "0");//visible
                }

                xmlEle.SetAttribute("ID", row["ID"].ToString());
                xmlEle.SetAttribute("WIDTH", iWidth.ToString());
                xmlEle.SetAttribute("HEIGHT", iHeight.ToString());

                xmlTmp1 = xmlDoc.CreateElement("IMAGESRC");
                xmlTmp1.InnerText = row["imageurl"].ToString() == "" ? "../Images/EquImage/net.jpg" : row["imageurl"].ToString();

                xmlEle.AppendChild(xmlTmp1);

                xmlTmp1 = xmlDoc.CreateElement("TEXT");
                xmlTmp1.InnerText = row["equname"].ToString();
                xmlEle.AppendChild(xmlTmp1);



                if (strType == "4")
                {
                    if (iflg == 0)
                    {
                        if (sList.IndexOf(row["relid"].ToString() + ",") < 0)
                        {
                            iCount++;
                            //未出现过则递归，防止死循环
                            sList += row["relid"].ToString() + ",";

                            //递归加后面的
                            AddNextRelEqu(xmlDoc, dt, long.Parse(row["Equ_ID"].ToString()), iTmpBeginX, iTmpBeginY, iWidth, iHeight, iWStep, iHStep, iLay, ref iTotlIndex, ref iTotalStepTmp, ref xmlRoot, ref sList, strType);

                        }
                    }
                    else
                    {
                        if (iCount == 0)
                        {
                            //如果是同一次循环，且第一次进入；则说明实际中的第一次没能进入上面的循环【已经循环过了，所以这次也不应该进入循环】
                            break;
                        }
                        else
                        {
                            iCount++;
                            AddNextRelEqu(xmlDoc, dt, long.Parse(row["Equ_ID"].ToString()), iTmpBeginX, iTmpBeginY, iWidth, iHeight, iWStep, iHStep, iLay, ref iTotlIndex, ref iTotalStepTmp, ref xmlRoot, ref sList, strType);
                        }
                    }
                }
                else
                {
                    if (sList.IndexOf(row["relid"].ToString() + ",") < 0)
                    {
                        //未出现过则递归，防止死循环
                        sList += row["relid"].ToString() + ",";

                        //递归加后面的
                        AddNextRelEqu(xmlDoc, dt, long.Parse(row["relid"].ToString()), iTmpBeginX, iTmpBeginY, iWidth, iHeight, iWStep, iHStep, iLay, ref iTotlIndex, ref iTotalStepTmp, ref xmlRoot, ref sList, strType);

                    }
                }


                //记录画线前 子循环的步长
                if (iIndex > 0)
                    iPreTotalStepTemp = iTotalStepTmp;

                //iTotalStep = iTotalStepTmp;

                ////后面的小过INDEX 时 间隔增1
                //if (iIndex >= iTotalStep)
                //    iTotalStep++;


                //下一次循环 的第一个环节起点 Y间隔 及这次循环 连线的Y间隔
                iPrePreTotalStep = iPreTotalStep;
                iPreTotalStep = iPreTotalStep + iTotalStepTmp;
                iTotalStep = iPreTotalStep;


                xmlRoot.AppendChild(xmlEle);

                xmlEle = xmlDoc.CreateElement("LINK");

                xmlTmp1 = xmlDoc.CreateElement("TEXT");


                iTmp = iBeginX + iWidth + 200;
                xmlTmp1.SetAttribute("X", iTmp.ToString());

                if (iIndex > 0)
                {
                    iTmp = iBeginY + iPrePreTotalStep * iHStep + iHeight / 2 - 250;
                }
                else
                {
                    iTmp = iBeginY + iHeight / 2 - 250;
                }
                xmlTmp1.SetAttribute("Y", iTmp.ToString());

                xmlTmp1.InnerText = row["reldescription"].ToString();

                xmlEle.AppendChild(xmlTmp1);

                #region 下方文字
                xmlTmp1 = xmlDoc.CreateElement("TEXT2");

                iTmp = iBeginX + iWidth;
                xmlTmp1.SetAttribute("X", iTmp.ToString());

                if (iIndex > 0)
                {
                    iTmp = iBeginY + iPrePreTotalStep * iHStep + iHeight / 2 + 100;
                }
                else
                {
                    iTmp = iBeginY + iHeight / 2 + 100;
                }

                xmlTmp1.SetAttribute("Y", iTmp.ToString());

                xmlTmp1.InnerText = Equ_DeskDP.GetEquNameByID(row["RelID"].ToString()) + ":[" + GetPropNamesByID(row["ID"].ToString(), row["RelID"].ToString(), row["RelKey"].ToString()) + "]";

                xmlEle.AppendChild(xmlTmp1);
                #endregion

                xmlTmp1 = xmlDoc.CreateElement("_DRAWSTYLE");
                xmlTmp1.InnerText = "Solid";
                xmlEle.AppendChild(xmlTmp1);

                xmlTmp1 = xmlDoc.CreateElement("_ARROWDST");
                xmlTmp1.InnerText = "Classic";
                xmlEle.AppendChild(xmlTmp1);

                xmlTmp1 = xmlDoc.CreateElement("EXTRAPOINTS");

                // 第一个关系 2个点，第2个关系起 三个点
                if (iIndex > 0)
                {
                    //有竖线
                    XmlElement xmlTmp2 = xmlDoc.CreateElement("POINT");
                    iTmp = iBeginX + iWidth;
                    xmlTmp2.SetAttribute("X", iTmp.ToString());
                    if (iIndex == 1)
                    {
                        //第2个时，竖线从起点开始
                        iTmp = iBeginY + iHeight / 2;
                    }
                    else
                    {
                        //iTmp = iBeginY + (iPrePreTotalStep - iPreTotalStepTemp) * iHStep + iHeight / 2;
                        iTmp = iBeginY + iHeight / 2;
                    }
                    xmlTmp2.SetAttribute("Y", iTmp.ToString());

                    xmlTmp1.AppendChild(xmlTmp2);

                    xmlTmp2 = xmlDoc.CreateElement("POINT");
                    iTmp = iBeginX + iWidth;
                    xmlTmp2.SetAttribute("X", iTmp.ToString());
                    iTmp = iBeginY + iPrePreTotalStep * iHStep + iHeight / 2;
                    xmlTmp2.SetAttribute("Y", iTmp.ToString());

                    xmlTmp1.AppendChild(xmlTmp2);

                    //尾节点
                    xmlTmp2 = xmlDoc.CreateElement("POINT");

                    iTmp = iBeginX + iWStep;
                    xmlTmp2.SetAttribute("X", iTmp.ToString());
                    iTmp = iBeginY + iPrePreTotalStep * iHStep + iHeight / 2;
                    xmlTmp2.SetAttribute("Y", iTmp.ToString());



                    xmlTmp1.AppendChild(xmlTmp2);



                }
                else
                {
                    //无竖线
                    XmlElement xmlTmp2 = xmlDoc.CreateElement("POINT");
                    iTmp = iBeginX + iWidth;
                    xmlTmp2.SetAttribute("X", iTmp.ToString());
                    iTmp = iBeginY + iHeight / 2;
                    xmlTmp2.SetAttribute("Y", iTmp.ToString());

                    xmlTmp1.AppendChild(xmlTmp2);



                    //尾节点
                    xmlTmp2 = xmlDoc.CreateElement("POINT");

                    iTmp = iBeginX + iWStep;
                    xmlTmp2.SetAttribute("X", iTmp.ToString());
                    iTmp = iBeginY + iHeight / 2;
                    xmlTmp2.SetAttribute("Y", iTmp.ToString());

                    xmlTmp1.AppendChild(xmlTmp2);

                }

                xmlEle.AppendChild(xmlTmp1);

                xmlRoot.AppendChild(xmlEle);
                iIndex++;



            }



        }

        #endregion


        #region InsertRecorded
        /// <summary>
        /// 
        /// </summary>
        /// <param name=pEqu_RelDP></param>
        public void InsertRecorded(Equ_RelDP pEqu_RelDP)
        {
            OracleConnection cn = ConfigTool.GetConnection();
            string strSQL = string.Empty;
            string strID = "0";
            try
            {
                strSQL = @"INSERT INTO Equ_Rel(
									Equ_ID,
									RelID,
                                    RelPropID,
                                    RelPropName,
									RelDescription
					)
					VALUES( " +
                            pEqu_RelDP.Equ_ID.ToString() + "," +
                            pEqu_RelDP.RelID.ToString() + "," +
                            pEqu_RelDP.RelPropID.ToString() + "," +
                            pEqu_RelDP.RelPropName.ToString() + "," +
                            StringTool.SqlQ(pEqu_RelDP.RelDescription) +
                    ")";

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

        #region UpdateRecorded
        /// <summary>
        /// 
        /// </summary>
        /// <param name=pEqu_RelDP></param>
        public void UpdateRecorded(Equ_RelDP pEqu_RelDP)
        {
            OracleConnection cn = ConfigTool.GetConnection();
            string strSQL = string.Empty;
            try
            {
                strSQL = @"UPDATE Equ_Rel Set " +
                                                        " Equ_ID = " + pEqu_RelDP.Equ_ID.ToString() + "," +
                            " RelID = " + pEqu_RelDP.RelID.ToString() + "," +
                            " RelPropID = " + pEqu_RelDP.RelPropID.ToString() + "," +
                            " RelPropName = " + StringTool.SqlQ(pEqu_RelDP.RelPropName.ToString()) + "," +
                            " RelDescription = " + StringTool.SqlQ(pEqu_RelDP.RelDescription) +
                                " WHERE ID = " + pEqu_RelDP.ID.ToString();

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
        /// <param name="lngID"></param>
        public void DeleteRecorded(long lngID)
        {
            OracleConnection cn = ConfigTool.GetConnection();
            try
            {
                string strSQL = "Update Equ_Rel Set Deleted=1  WHERE ID =" + lngID.ToString();
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

        #region 批量保存 SaveItem
        /// <summary>
        /// 批量保存
        /// </summary>
        /// <param name="dt"></param>
        /// <param name="sEqu_ID"></param>
        public static void SaveItem(DataTable dt, decimal sEqu_ID)
        {
            OracleConnection cn = ConfigTool.GetConnection();
            try
            {
                System.Text.StringBuilder sql = new System.Text.StringBuilder();
                sql.Append("Delete From Equ_Rel Where Equ_ID=" + sEqu_ID.ToString());
                foreach (DataRow dr in dt.Rows)
                {
                    sql.Append(" Insert Into Equ_Rel(Equ_ID,RelID,RelPropID,RelPropName,RelDescription) Values(");
                    sql.AppendFormat(sEqu_ID.ToString() + "," + dr["RelID"].ToString() + "," + dr["RelPropID"].ToString() + "," + StringTool.SqlQ(dr["RelPropName"].ToString()) + "," + StringTool.SqlQ(dr["RelDescription"].ToString()) + ")");
                }
                if (!string.IsNullOrEmpty(sql.ToString()))
                {
                    OracleDbHelper.ExecuteNonQuery(cn, CommandType.Text, sql.ToString());
                }
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
        /// <param name="dt"></param>
        /// <param name="sEqu_ID"></param>
        /// <param name="lngUserID"></param>
        public static void SaveItem(DataTable dt, decimal sEqu_ID, long lngUserID)
        {
            OracleConnection cn = ConfigTool.GetConnection();
            if (cn.State != ConnectionState.Open)
                cn.Open();
            OracleTransaction trans = cn.BeginTransaction();
            try
            {
                string sql = string.Empty;
                sql = "Delete From Equ_Rel Where Equ_ID=" + sEqu_ID.ToString();
                OracleDbHelper.ExecuteNonQuery(trans, CommandType.Text, sql.ToString());
                foreach (DataRow dr in dt.Rows)
                {
                    string strID = EpowerGlobal.EPGlobal.GetNextID("Equ_Rel_SEQUENCE").ToString();

                    sql = " Insert Into Equ_Rel(ID,Equ_ID,RelID,RelPropID,RelPropName,RelDescription,ReldescriptionID, RelKey) Values(" + strID + ",";
                    sql += sEqu_ID.ToString() + "," + dr["RelID"].ToString() + "," + dr["RelPropID"].ToString() + "," + StringTool.SqlQ(dr["RelPropName"].ToString()) + "," + StringTool.SqlQ(dr["RelDescription"].ToString()) + ","
                        + StringTool.SqlQ(dr["ReldescriptionID"].ToString()) + ", " + StringTool.SqlQ(dr["RelKey"].ToString()) + ")";
                    OracleDbHelper.ExecuteNonQuery(trans, CommandType.Text, sql.ToString());


                    UpdateIdiomForSchemaValues(dr["RelDescription"].ToString(), lngUserID);
                }

                trans.Commit();
            }
            catch (Exception e)
            {
                trans.Rollback();
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
        /// <param name="dt"></param>
        /// <param name="sEqu_ID"></param>
        /// <param name="lngUserID"></param>
        public static void SaveItemNew2222(DataTable dt, decimal sEqu_ID, long lngUserID, String strRelKey)
        {
            OracleConnection cn = ConfigTool.GetConnection();
            if (cn.State != ConnectionState.Open)
                cn.Open();
            //OracleTransaction trans = cn.BeginTransaction();
            try
            {
                string sql = string.Empty;
                sql = "Delete From Equ_Rel Where Equ_ID=" + sEqu_ID.ToString();
                OracleDbHelper.ExecuteNonQuery(cn, CommandType.Text, sql.ToString());
                foreach (DataRow dr in dt.Rows)
                {
                    string strID = EpowerGlobal.EPGlobal.GetNextID("Equ_Rel_SEQUENCE").ToString();

                    sql = " Insert Into Equ_Rel(ID,Equ_ID,RelID,RelPropID,RelPropName,RelDescription,ReldescriptionID, RelKey) Values(" + strID + ",";
                    sql += sEqu_ID.ToString() + "," + dr["RelID"].ToString() + "," + dr["RelPropID"].ToString() + "," + StringTool.SqlQ(dr["RelPropName"].ToString()) + "," + StringTool.SqlQ(dr["RelDescription"].ToString()) + ","
                        + StringTool.SqlQ(dr["ReldescriptionID"].ToString()) + ", " + StringTool.SqlQ(dr["RelKey"].ToString()) + ")";
                    OracleDbHelper.ExecuteNonQuery(cn, CommandType.Text, sql.ToString());


                    UpdateIdiomForSchemaValues(dr["RelDescription"].ToString(), lngUserID);
                }

                //  trans.Commit();
            }
            catch (Exception e)
            {
                //trans.Rollback();
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
        /// <param name="dt"></param>
        /// <param name="sEqu_ID"></param>
        /// <param name="lngUserID"></param>
        public static void SaveItemNew2(DataTable dt, decimal sEqu_ID, long lngUserID)
        {
            OracleConnection cn = ConfigTool.GetConnection();
            if (cn.State != ConnectionState.Open)
                cn.Open();
            OracleTransaction trans = cn.BeginTransaction();
            try
            {
                string sql = string.Empty;
                sql = "Delete From Equ_Rel Where Equ_ID=" + sEqu_ID.ToString();
                OracleDbHelper.ExecuteNonQuery(trans, CommandType.Text, sql.ToString());
                foreach (DataRow dr in dt.Rows)
                {
                    string strID = EpowerGlobal.EPGlobal.GetNextID("Equ_Rel_SEQUENCE").ToString();

                    sql = " Insert Into Equ_Rel(ID,Equ_ID,RelID,RelPropID,RelPropName,RelDescription,ReldescriptionID, RelKey) Values("+ strID +",";
                    sql += sEqu_ID.ToString() + "," + dr["RelID"].ToString() + "," + dr["RelPropID"].ToString() + "," + StringTool.SqlQ(dr["RelPropName"].ToString()) + "," + StringTool.SqlQ(dr["RelDescription"].ToString()) + ","
                        + StringTool.SqlQ(dr["ReldescriptionID"].ToString()) + ", " + StringTool.SqlQ(dr["RelKey"].ToString()) + ")";
                    OracleDbHelper.ExecuteNonQuery(trans, CommandType.Text, sql.ToString());


                    UpdateIdiomForSchemaValues(dr["RelDescription"].ToString(), lngUserID);
                }

                trans.Commit();
            }
            catch (Exception e)
            {
                trans.Rollback();
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
        /// <param name="dt"></param>
        /// <param name="sEqu_ID"></param>
        /// <param name="lngUserID"></param>
        public static void SaveItemNew(DataTable dt, decimal sEqu_ID, long lngUserID,
            String strRelKey)
        {
            OracleConnection cn = ConfigTool.GetConnection();
            if (cn.State != ConnectionState.Open)
                cn.Open();
            OracleTransaction trans = cn.BeginTransaction();
            try
            {
                string sql = string.Empty;
                sql = String.Format("Delete From Equ_Rel Where Equ_ID={0} AND RelKey = {1}",
                    sEqu_ID, StringTool.SqlQ(strRelKey.ToLower()));

                OracleDbHelper.ExecuteNonQuery(trans, CommandType.Text, sql.ToString());
                foreach (DataRow dr in dt.Rows)
                {
                    string strID = EpowerGlobal.EPGlobal.GetNextID("Equ_Rel_SEQUENCE").ToString();

                    sql = " Insert Into Equ_Rel(ID,Equ_ID,RelID,RelPropID,RelPropName,RelDescription,ReldescriptionID, RelKey) Values("+ strID +",";
                    sql += sEqu_ID.ToString() + "," + dr["RelID"].ToString() + "," + dr["RelPropID"].ToString() + "," + StringTool.SqlQ(dr["RelPropName"].ToString()) + "," + StringTool.SqlQ(dr["RelDescription"].ToString()) + ","
                        + StringTool.SqlQ(dr["ReldescriptionID"].ToString()) + ", " + StringTool.SqlQ(dr["RelKey"].ToString()) + ")";
                    OracleDbHelper.ExecuteNonQuery(trans, CommandType.Text, sql.ToString());


                    UpdateIdiomForSchemaValues(dr["RelDescription"].ToString(), lngUserID);
                }

                trans.Commit();
            }
            catch (Exception e)
            {
                trans.Rollback();
                throw e;
            }
            finally
            {
                ConfigTool.CloseConnection(cn);
            }
        }
        /// <summary>
        /// 保存, 新版本. [sunshaozong@gmail.com]
        /// </summary>
        /// <param name="dt"></param>
        /// <param name="sEqu_ID"></param>
        /// <param name="lngUserID"></param>
        public static void SaveItemNew23(DataTable dt, decimal sEqu_ID, long lngUserID)
        {
            OracleConnection cn = ConfigTool.GetConnection();
            if (cn.State != ConnectionState.Open)
                cn.Open();
            OracleTransaction trans = cn.BeginTransaction();
            try
            {
                string sql = string.Empty;
                foreach (DataRow dr in dt.Rows)
                {
                    sql = String.Format("SELECT ID FROM EQU_REL WHERE RelID = {0} AND RelKey = {1} AND EQU_ID = {2}",
                        dr["RelID"], StringTool.SqlQ(dr["RelKey"].ToString()),
                        dr["Equ_ID"]);
                    //Object obj = OracleDbHelper.ExecuteScalar(trans, CommandType.Text, sql);
                    Object obj = null;
                    DataTable _dt = CommonDP.ExcuteSqlTable(sql);
                    if (_dt != null && _dt.Rows.Count > 0)
                    {
                        obj = _dt.Rows[0][0];
                    }

                    Int32 intRelID;
                    Boolean isOk = Int32.TryParse(obj != null ? obj.ToString() : "", out intRelID);
                    if (isOk)
                    {
                        sql = @"UPDATE EQU_REL SET EQU_ID = {0}, RelID = {1}, RelPropID = {2},
                                RelPropName = {3}, RelDescription = {4}, ReldescriptionID = {5}, RelKey = {6} 
                                WHERE RelID = {7}";
                        sql = String.Format(sql,
                            sEqu_ID, StringTool.SqlQ(dr["RelID"].ToString()),
                            StringTool.SqlQ(dr["RelPropID"].ToString()),
                            StringTool.SqlQ(dr["RelPropName"].ToString()),
                            StringTool.SqlQ(dr["RelDescription"].ToString()),
                            dr["ReldescriptionID"], StringTool.SqlQ(dr["RelKey"].ToString()),
                            StringTool.SqlQ(dr["RelID"].ToString()));
                        //sql = " Insert Into Equ_Rel(ID,Equ_ID,RelID,RelPropID,RelPropName,RelDescription,ReldescriptionID, RelKey) Values(Equ_Rel_SEQUENCE.nextval,";
                        //sql += sEqu_ID.ToString() + "," + dr["RelID"].ToString()
                        //    + "," + dr["RelPropID"].ToString() + ","
                        //    + StringTool.SqlQ(dr["RelPropName"].ToString()) + ","
                        //    + StringTool.SqlQ(dr["RelDescription"].ToString()) + ","
                        //    + StringTool.SqlQ(dr["ReldescriptionID"].ToString()) + ", "
                        //    + StringTool.SqlQ(dr["RelKey"].ToString()) + ")";
                        OracleDbHelper.ExecuteNonQuery(trans, CommandType.Text, sql);

                        UpdateIdiomForSchemaValues(dr["RelDescription"].ToString(), lngUserID);
                    }
                    else
                    {
                        string strID = EpowerGlobal.EPGlobal.GetNextID("Equ_Rel_SEQUENCE").ToString();

                        sql = " Insert Into Equ_Rel(ID,Equ_ID,RelID,RelPropID,RelPropName,RelDescription,ReldescriptionID, RelKey) Values(" + strID + ",";
                        sql += sEqu_ID.ToString() + "," + dr["RelID"].ToString() + "," + dr["RelPropID"].ToString() + "," + StringTool.SqlQ(dr["RelPropName"].ToString()) + "," + StringTool.SqlQ(dr["RelDescription"].ToString()) + ","
                            + StringTool.SqlQ(dr["ReldescriptionID"].ToString()) + ", " + StringTool.SqlQ(dr["RelKey"].ToString()) + ")";
                        OracleDbHelper.ExecuteNonQuery(trans, CommandType.Text, sql.ToString());

                        UpdateIdiomForSchemaValues(dr["RelDescription"].ToString(), lngUserID);
                    }
                }

                trans.Commit();
            }
            catch (Exception e)
            {
                trans.Rollback();
                throw e;
            }
            finally
            {
                ConfigTool.CloseConnection(cn);
            }
        }

        #region 保存多个不同资产的关联资产
        /// <summary>
        /// 保存多个不同资产的关联资产
        /// </summary>
        /// <param name="dt"></param>
        /// <param name="sEqu_ID">多个相同配置项值的被关联资产ID串</param>
        /// <param name="lngUserID"></param>
        public static void SaveItem(DataTable dt, string sEqu_ID, long lngUserID)
        {
            OracleConnection cn = ConfigTool.GetConnection();

            string[] strArrEquID = sEqu_ID.Split(',');          //资产ID串
            try
            {
                System.Text.StringBuilder sqlDel = new System.Text.StringBuilder();
                string strSqlDel = string.Empty;                    //删除sql语句
                string strWhereDel = string.Empty;                  //删除where条件
                //先删除这两个资产之前共有的关联资产属性
                for (int i = 0; i < strArrEquID.Length; i++)
                {
                    strSqlDel = " delete from Equ_Rel where Equ_ID = " + strArrEquID[i];
                    for (int j = 0; j < strArrEquID.Length; j++)
                    {
                        if (j != i)
                        {
                            strWhereDel += "  and RelID in (select RelID from Equ_Rel where Equ_ID = " + strArrEquID[j] + ") ";
                        }
                    }
                    sqlDel.Append(strSqlDel);
                }
                if (!string.IsNullOrEmpty(sqlDel.ToString()))
                {
                    OracleDbHelper.ExecuteNonQuery(cn, CommandType.Text, sqlDel.ToString());
                }

                System.Text.StringBuilder sql = new System.Text.StringBuilder();
                for (int i = 0; i < strArrEquID.Length; i++)
                {
                    foreach (DataRow dr in dt.Rows)
                    {
                        sql.Append(" Insert Into Equ_Rel(Equ_ID,RelID,RelPropID,RelPropName,RelDescription,ReldescriptionID) Values(");
                        sql.AppendFormat(strArrEquID[i].ToString() + "," + dr["RelID"].ToString() + "," + dr["RelPropID"].ToString() + "," + StringTool.SqlQ(dr["RelPropName"].ToString()) + "," + StringTool.SqlQ(dr["RelDescription"].ToString()) + "," + StringTool.SqlQ(dr["ReldescriptionID"].ToString()) + ")");
                        UpdateIdiomForSchemaValues(dr["RelDescription"].ToString(), lngUserID);
                    }
                }

                if (!string.IsNullOrEmpty(sql.ToString()))
                {
                    OracleDbHelper.ExecuteNonQuery(cn, CommandType.Text, sql.ToString());
                }
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

        #region 保存多个不同资产的关联资产
        /// <summary>
        /// 保存多个不同资产的关联资产
        /// </summary>
        /// <param name="sEqu_ID">批量被关联资产ID串</param>
        /// <param name="lngUserID"></param>
        /// <param name="strRelID">关联资产ID</param>
        /// <param name="strRelPropID">关联资产属性ID</param>
        /// <param name="strRelPropName">关联资产属性</param>
        /// <param name="strRelDesID">关联描述ID</param>
        /// <param name="strRelDes">关联描述</param>
        public static void SaveItem(string sEqu_ID, long lngUserID, string strRelID, string strRelPropID, string strRelPropName, string strRelDesID, string strRelDes)
        {
            OracleConnection cn = ConfigTool.GetConnection();

            if (sEqu_ID != string.Empty)
            {
                string[] strArrEquID = sEqu_ID.Split(',');          //资产ID串
                try
                {
                    System.Text.StringBuilder sqlDel = new System.Text.StringBuilder();
                    string strSqlDel = string.Empty;                    //删除sql语句
                    string strWhereDel = string.Empty;                  //删除where条件
                    //先删除这两个资产之前共有的关联资产属性
                    for (int i = 0; i < strArrEquID.Length; i++)
                    {
                        strSqlDel = " delete from Equ_Rel where Equ_ID = " + strArrEquID[i];
                        for (int j = 0; j < strArrEquID.Length; j++)
                        {
                            if (j != i)
                            {
                                strWhereDel += "  and RelID in (select RelID from Equ_Rel where Equ_ID = " + strArrEquID[j] + ") ";
                            }
                        }
                        sqlDel.Append(strSqlDel);
                    }
                    if (!string.IsNullOrEmpty(sqlDel.ToString()))
                    {
                        OracleDbHelper.ExecuteNonQuery(cn, CommandType.Text, sqlDel.ToString());
                    }

                    System.Text.StringBuilder sql = new System.Text.StringBuilder();
                    for (int i = 0; i < strArrEquID.Length; i++)
                    {
                        sql.Append(" Insert Into Equ_Rel(Equ_ID,RelID,RelPropID,RelPropName,RelDescription,ReldescriptionID) Values(");
                        sql.AppendFormat(strArrEquID[i].ToString() + "," + strRelID + "," + strRelPropID + "," + StringTool.SqlQ(strRelPropName) + "," + StringTool.SqlQ(strRelDes) + "," + StringTool.SqlQ(strRelDesID) + ")");
                        UpdateIdiomForSchemaValues(strRelDes, lngUserID);
                    }

                    if (!string.IsNullOrEmpty(sql.ToString()))
                    {
                        OracleDbHelper.ExecuteNonQuery(cn, CommandType.Text, sql.ToString());
                    }
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
        #endregion

        #region 不选择任何资产时，将原有的关联删除
        /// <summary>
        /// 不选择任何资产时，将原有的关联删除
        /// </summary>
        /// <param name="sEqu_ID">批量被关联资产ID串</param>
        /// <param name="strRelID">关联资产ID</param>
        /// <param name="strRelPropID">关联资产属性ID</param>
        /// <param name="strRelPropName">关联资产属性</param>
        /// <param name="strRelDesID">关联描述ID</param>
        /// <param name="strRelDes">关联描述</param>
        public static void SaveItem(string sEqu_ID, string strRelID, string strRelPropID, string strRelPropName, string strRelDesID, string strRelDes)
        {
            OracleConnection cn = ConfigTool.GetConnection();

            if (sEqu_ID != string.Empty)
            {
                string[] strArrEquID = sEqu_ID.Split(',');          //资产ID串
                try
                {
                    System.Text.StringBuilder sqlDel = new System.Text.StringBuilder();
                    string strSqlDel = string.Empty;                    //删除sql语句
                    string strWhereDel = string.Empty;                  //删除where条件
                    //先删除这两个资产之前共有的关联资产属性
                    for (int i = 0; i < strArrEquID.Length; i++)
                    {
                        strSqlDel = " delete from Equ_Rel where Equ_ID = " + strArrEquID[i];
                        for (int j = 0; j < strArrEquID.Length; j++)
                        {
                            if (j != i)
                            {
                                strWhereDel += "  and RelID in (select RelID from Equ_Rel where Equ_ID = " + strArrEquID[j] + ") ";
                            }
                        }
                        sqlDel.Append(strSqlDel);
                    }
                    if (!string.IsNullOrEmpty(sqlDel.ToString()))
                    {
                        OracleDbHelper.ExecuteNonQuery(cn, CommandType.Text, sqlDel.ToString());
                    }
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
        #endregion

        #endregion

        /// <summary>
        /// 更新配置项习惯用语存储
        /// </summary>
        /// <param name="sXml"></param>
        /// <param name="lngUserID"></param>
        private static void UpdateIdiomForSchemaValues(string sFieldValue, long lngUserID)
        {
            IdiomDP.AddIdiom(lngUserID, "EquRel_RelDescription", sFieldValue);
        }

        #region 新增取得数据
        /// <summary>
        /// 
        /// </summary>
        /// <param name="sArr"></param>
        /// <param name="sEquID"></param>
        /// <returns></returns>
        public static DataTable GetProblemAnalsys(string sArr, string sEquID)
        {
            string sWhere = string.Empty;
            string[] arr = sArr.Split(',');
            if (arr.Length > 1)
                sWhere += " and Equ_Desk.ID In(";
            for (int i = 0; i < arr.Length - 1; i++)
            {
                if (i != arr.Length - 2)
                    sWhere += arr[i] + ",";
                else
                    sWhere += arr[i] + ")";
            }
            string strSQL = @"SELECT to_number(" + StringTool.SqlQ(sEquID) + " ) as Equ_ID,'' as  RelDescription,Equ_Desk.ID as RelID,Name,Code"
                + " FROM Equ_Desk WHERE 1=1 ";
            strSQL += sWhere;
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
        #endregion

        #region 新增取得数据【批量】
        /// <summary>
        /// 新增取得数据【批量
        /// </summary>
        /// <param name="sEquID"></param>
        /// <returns></returns>
        public static DataTable GetProblemAnalsys(string sEquID)
        {
            string strSQL = @"SELECT  to_number(" + StringTool.SqlQ(sEquID) + ") as RelID,'' as  RelDescription,Name"
                + " FROM Equ_Desk WHERE ROWNUM<=1 AND  ID = " + sEquID;
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
        #endregion

        #region 根据关联主表ID删除关联记录
        /// <summary>
        /// 根据关联主表ID删除关联记录
        /// </summary>
        /// <param name="strEquRelID"></param>
        /// <returns></returns>
        public static bool DelEquRelByRelID(string strEquRelID)
        {
            string strSql = "delete from equ_rel where ID = " + strEquRelID;

            try
            {
                CommonDP.ExcuteSql(strSql);

                return true;
            }
            catch
            {
                return false;
            }
        }
        #endregion

    }
}

