using System;
using System.Data;
using System.Data.OracleClient;
using Epower.DevBase.BaseTools;
using Epower.ITSM.Base;
using Epower.DevBase.Organization.SqlDAL;
using Epower.DevBase.Organization.Base;

namespace Epower.ITSM.SqlDAL
{
	/// <summary>
	/// NewsDp 的摘要说明。
	/// </summary>
	public class NewsDp
	{
        /// <summary>
        /// 
        /// </summary>
		public NewsDp()
		{
			//
			// TODO: 在此处添加构造函数逻辑
			//
		}


		/// <summary>
		/// 获取公告附件名称
		/// </summary>
		/// <param name="lngID"></param>
		/// <returns></returns>
		public static string GetFileName(long lngID)
		{
			string strSQL;

			string strRet="";

			strSQL="SELECT softname FROM oa_news WHERE newsid = " + lngID.ToString();

			OracleConnection cn = ConfigTool.GetConnection();

            try
            {
                OracleDataReader dr = OracleDbHelper.ExecuteReader(cn, CommandType.Text, strSQL);

                while (dr.Read())
                {
                    strRet = dr.GetString(0);
                    break;
                }
                dr.Close();
                
                return strRet;
            }
            finally { ConfigTool.CloseConnection(cn); }

		}

		/// <summary>
		/// 获取所有信息类别信息
		/// </summary>
		public static DataTable GetNewType()
		{
			string strSQL = "SELECT TypeId,TypeName,IsInner,IsOuter FROM OA_News_type ORDER BY IndexID,TypeName";

			DataTable dt;
			OracleConnection cn = ConfigTool.GetConnection();

			dt = OracleDbHelper.ExecuteDataTable(cn,CommandType.Text,strSQL);

			ConfigTool.CloseConnection(cn);

			return dt;
			
		}

		/// <summary>
		/// 获取所有信息类别详细信息
		/// </summary>
		public static DataTable GetNewTypedetail()
		{
			string strSQL = "SELECT TypeId,TypeName,"+
				" CASE WHEN IsInner ="+(int)eOA_IsInner.eTrue+" THEN '是'"+
				" ELSE '否' END IsInner,"+
				" CASE WHEN IsOuter ="+ (int)eOA_IsFocus.eTrue+" THEN '是' ELSE '否' END IsOuter ,"+
				" Description"+
				" FROM OA_News_type ORDER BY IndexID,TypeName";

			DataTable dt;
			OracleConnection cn = ConfigTool.GetConnection();

			dt = OracleDbHelper.ExecuteDataTable(cn,CommandType.Text,strSQL);

			ConfigTool.CloseConnection(cn);

			return dt;
			
		}


        /// <summary>
        /// 公告维护取公告信息
        /// </summary>
        /// <param name="lngOrgID"></param>
        /// <param name="lngUserID"></param>
        /// <param name="lngDeptID"></param>
        /// <param name="re"></param>
        /// <returns></returns>
        public static DataTable GetNewsdetail_two(long lngOrgID, long lngUserID, long lngDeptID, RightEntity re,int pagesize, 
                                                  int pageindex, ref int rowcount)
        {
            string strList = string.Empty;
            string strSQL = "";

            DataTable dt;
            OracleConnection cn = ConfigTool.GetConnection();

            try
            {
                if (re != null)
                {
                    strSQL = " 1=1 ";
                    if (re.CanRead == false)
                    {
                        //查询空
                        strSQL += " And 1=-1 ";
                    }
                    else
                    {
                        #region 范围条件
                        switch (re.RightRange)
                        {
                            case eO_RightRange.eFull:
                                strSQL += "";
                                break;
                            case eO_RightRange.ePersonal:
                                strSQL += " AND InputUser= " + lngUserID.ToString();
                                break;
                            case eO_RightRange.eDeptDirect:
                                strSQL += " AND InDeptID= " + lngDeptID.ToString();
                                break;
                            case eO_RightRange.eOrgDirect:
                                strSQL += " AND InOrgID= " + lngOrgID.ToString();
                                break;
                            case eO_RightRange.eDept:
                                strList = DeptDP.GetDeptFullID(lngDeptID);
                                if (strList.Trim().Length > 0)
                                {
                                    //不是根部门并有找到
                                    strSQL += "AND InDeptID in (select deptid from ts_dept where fullid like " + StringTool.SqlQ(strList + "%") + ")";
                                }
                                break;
                            case eO_RightRange.eOrg:
                                strList = DeptDP.GetDeptFullID(lngOrgID);
                                if (strList.Trim().Length > 0)
                                {
                                    //不是根部门并有找到
                                    strSQL += "AND InOrgID in (select distinct deptid from ts_dept where deptkind = 1 and fullid like " + StringTool.SqlQ(strList + "%") + ")";
                                }
                                break;
                            default:
                                strSQL += "";
                                break;
                        }
                        #endregion
                        //strSQL += " order by n.PubDate desc";
                    }
                }
                else
                {
                    //查询空
                    strSQL += " And 1=-1 ";
                }

                dt = OracleDbHelper.ExecuteDataTable(cn, "V_Newsdetail", "*", "order by flowid", pagesize, pageindex, strSQL, ref rowcount);

                return dt;
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            { 
             ConfigTool.CloseConnection(cn);
            }
        }


		/// <summary>
        /// 公告维护取公告信息
		/// </summary>
		/// <param name="lngOrgID"></param>
		/// <param name="lngUserID"></param>
		/// <param name="lngDeptID"></param>
		/// <param name="re"></param>
		/// <returns></returns>
        public static DataTable GetNewsdetail(long lngOrgID,long lngUserID,long lngDeptID, RightEntity re)
		{
            string strList = string.Empty;
			string strSQL = "SELECT nt.TypeName,n.NewsId,n.FileName,n.SoftName,n.Title,n.Writer,n.OutDate,"+
				" CASE WHEN n.DispFlag = 1 THEN '是' ELSE '否' END DispFlag,n.PubDate,InputUser,ts.name,"+
				" n.InOrgID,n.InDeptID,n.IsInner IsInnerN,"+
				" nt.TypeId "+
				" FROM OA_NEWS n,OA_NEWS_TYPE nt,ts_user ts "+
                " WHERE n.TypeId=nt.TypeId and n.InputUser=ts.userid ";
            if (re != null)
            {
                if (re.CanRead == false)
                {
                    //查询空
                    strSQL += " And 1=-1 ";
                }
                else
                {
                    #region 范围条件
                    switch (re.RightRange)
                    {
                        case eO_RightRange.eFull:
                            strSQL += "";
                            break;
                        case eO_RightRange.ePersonal:
                            strSQL += " AND n.InputUser= " + lngUserID.ToString();
                            break;
                        case eO_RightRange.eDeptDirect:
                            strSQL += " AND n.InDeptID= " + lngDeptID.ToString();
                            break;
                        case eO_RightRange.eOrgDirect:
                            strSQL += " AND n.InOrgID= " + lngOrgID.ToString();
                            break;
                        case eO_RightRange.eDept:
                            strList = DeptDP.GetDeptFullID(lngDeptID);
                            if (strList.Trim().Length > 0)
                            {
                                //不是根部门并有找到
                                strSQL += "AND n.InDeptID in (select deptid from ts_dept where fullid like "+StringTool.SqlQ(  strList+ "%")+")";
                            }
                            break;
                        case eO_RightRange.eOrg:
                            strList = DeptDP.GetDeptFullID(lngOrgID);
                            if (strList.Trim().Length > 0)
                            {
                                //不是根部门并有找到
                                strSQL += "AND n.InOrgID in (select distinct deptid from ts_dept where deptkind = 1 and fullid like "+StringTool.SqlQ(  strList+ "%")+")";
                            }
                            break;
                        default:
                            strSQL += "";
                            break;
                    }
                    #endregion
                    strSQL +=  " order by n.PubDate desc" ;
                }
            }
            else
            {
                //查询空
                strSQL += " And 1=-1 ";
            }

			DataTable dt;
			OracleConnection cn = ConfigTool.GetConnection();

			dt = OracleDbHelper.ExecuteDataTable(cn,CommandType.Text,strSQL);

			ConfigTool.CloseConnection(cn);

			return dt;
			
		}

        /// <summary>
        /// 获取所有最新公告目录
        /// </summary>
        public static DataTable GetBulletin(long lngOrgID, long lngDeptID)
        {
            string strSQL = "SELECT nt.TypeID,nt.TypeName,n.NewsId,n.FileName,n.SoftName,n.Title,n.Writer,n.DispFlag,n.PubDate,nvl(n.photo,' ') photo,n.InOrgID,n.InDeptID,n.IsInner IsInnerN," +
                "n.InputDate,nt.isinner,nt.isouter" +
                " FROM OA_NEWS n,OA_NEWS_TYPE nt,ts_dept b " +
                " WHERE n.OutDate>=sysdate And n.TypeId=nt.TypeId and pubdate<=sysdate and " +
                " n.inORgid = b.deptid AND" +
                " ((n.isInner=" + (int)eOA_ReadRange.OrgRange + " AND n.InOrgID=" + lngOrgID.ToString() + ") OR" +
                " (n.isInner=" + (int)eOA_ReadRange.DeptRange + " AND n.InDeptID=" + lngDeptID.ToString() + ") OR" +
                " ((EXISTS (SELECT d.ORgid " +
                " FROM ts_dept d WHERE ROWNUM<=20 and substr(d.fullid,1,length(b.fullid)) = b.fullid AND d.fullid <> b.fullid AND" +
                " d.ORgid = " + lngOrgID.ToString() + ") OR n.InDeptID = " + lngDeptID.ToString() + " ) OR n.isinner=1)) and n.DispFlag=1 and n.Flag=1 " +
                " ORDER BY nt.IndexID,NewsId desc";
            DataTable dt;
            OracleConnection cn = ConfigTool.GetConnection("SQLConnString");

            dt = OracleDbHelper.ExecuteDataTable(cn, CommandType.Text, strSQL);

            ConfigTool.CloseConnection(cn);

            return dt;
        }
        /// <summary>
        /// 获取所有最新需要弹出的公告
        /// </summary>
        public static DataTable GetIsAlterNews(long lngOrgID, long lngDeptID)
        {
            string strSQL = "SELECT nt.TypeID,nt.TypeName,n.NewsId,n.FileName,n.SoftName,n.Title,n.Writer,n.DispFlag,n.PubDate,nvl(n.photo,' ') photo,n.InOrgID,n.InDeptID,n.IsInner IsInnerN," +
                "n.InputDate,n.IsAlert,n.content,nt.isinner,nt.isouter" +
                " FROM OA_NEWS n,OA_NEWS_TYPE nt,ts_dept b " +
                " WHERE n.OutDate>=sysdate And n.TypeId=nt.TypeId and pubdate<=sysdate and n.IsAlert=1 and n.Flag=1 and " +
                " n.inORgid = b.deptid AND" +
                " ((n.isInner=" + (int)eOA_ReadRange.OrgRange + " AND n.InOrgID=" + lngOrgID.ToString() + ") OR" +
                " (n.isInner=" + (int)eOA_ReadRange.DeptRange + " AND n.InDeptID=" + lngDeptID.ToString() + ") OR" +
                " ((EXISTS (SELECT d.ORgid " +
                " FROM ts_dept d WHERE ROWNUM<=20 and substr(d.fullid,1,length(b.fullid)) = b.fullid AND d.fullid <> b.fullid AND" +
                " d.ORgid = " + lngOrgID.ToString() + ") OR n.InDeptID = " + lngDeptID.ToString() + " ) OR n.isinner=1)) and n.DispFlag=1" +
                " ORDER BY nt.IndexID,NewsId desc";
            DataTable dt;
            OracleConnection cn = ConfigTool.GetConnection("SQLConnString");

            dt = OracleDbHelper.ExecuteDataTable(cn, CommandType.Text, strSQL);

            ConfigTool.CloseConnection(cn);

            return dt;
        }

		/// <summary>
        /// 展示公告
		/// </summary>
		/// <param name="RecordCount"></param>
		/// <param name="Range"></param>
		/// <param name="lngOrgID"></param>
		/// <param name="lngDeptID"></param>
		/// <param name="IsAll"></param>
		/// <returns></returns>
		public static DataTable GetShowNewsInner(int RecordCount,int Range,long lngOrgID,long lngDeptID,bool IsAll)
		{
            string strSQL = "";
            if (Range == 1)
            {
                strSQL = "SELECT nt.TypeID,nt.TypeName,n.NewsId,n.FileName,n.SoftName,n.Title,n.Writer,n.DispFlag,n.PubDate,n.photo," +
                    "n.InputDate,nt.isinner,nt.isouter,case when (n.outdate>=sysdate AND n.pubdate<=sysdate) then 0 else 1 end TimeOutFlag FROM oa_news n,OA_NEWS_TYPE nt" +
                    " WHERE ROWNUM<=" + RecordCount.ToString() + " and  n.TypeId=nt.TypeId AND n.DispFlag=1 AND n.InOrgID=1 and n.Flag=1 AND nt.IsInner=" + (int)eOA_IsInner.eTrue +
                    " AND n.IsInner=" + (int)eOA_ReadRange.AllRange;
                if (!IsAll)
                {
                    strSQL += "  And n.outdate>=sysdate AND n.pubdate<=sysdate ";
                }
                //" AND "+RecordCount.ToString()+">(SELECT COUNT(*) FROM oa_news b where b.newsid>n.newsid and b.typeid=n.typeid)"+
                strSQL += " order by nt.TypeName,n.PubDate desc";
            }
            else
            {
                strSQL = " SELECT nt.TypeID,nt.TypeName,n.NewsId,n.FileName,n.SoftName,n.Title,n.Writer,n.DispFlag,n.PubDate,n.photo," +
                    " n.InputDate,nt.isinner,nt.isouter,case when (n.outdate>=sysdate AND n.pubdate<=sysdate) then 0 else 1 end TimeOutFlag" +
                    " FROM oa_news n,OA_NEWS_TYPE nt,ts_dept t" +
                    " WHERE  n.TypeId=nt.TypeId  AND" +
                    " n.DispFlag=1 and n.Flag=1 AND " +
                    " n.inorgid = t.deptid AND" +
                    " ((n.IsInner=" + (int)eOA_ReadRange.OrgRange + " AND" +
                    " n.InOrgID=" + lngOrgID.ToString() + ") OR" +
                    " (n.IsInner=" + (int)eOA_ReadRange.DeptRange + " AND" +
                    " n.InDeptID=" + lngDeptID.ToString() + ") OR" +
                    " ((EXISTS (SELECT d.orgid " +
                    " FROM ts_dept d" +
                    " WHERE ROWNUM<=" + RecordCount.ToString() + " and substr(d.fullid,1,length(t.fullid)) = t.fullid AND" +
                    " d.fullid <> t.fullid AND" +
                    " d.orgid = " + lngOrgID.ToString() + ") OR" +
                    " n.InDeptID = " + lngDeptID.ToString() + ") AND" +
                    " n.IsInner=" + (int)eOA_ReadRange.AllRange + ") AND" +
                    " n.InOrgID<>1)  ";
                if (!IsAll)
                {
                    strSQL += " And n.outdate>=sysdate And n.pubdate<=sysdate";
                }
                //RecordCount.ToString()+">(SELECT COUNT(*) FROM oa_news b WHERE b.newsid>n.newsid AND b.typeid=n.typeid)"+  
                strSQL += " order by nt.TypeName,n.PubDate desc";
            }


            //			string strSQL = "DECLARE @strSQL varchar(8000) "+
            //							"DECLARE @strSubSQL varchar(300) "+
            //							"DECLARE @TypeID decimal(9) "+
            //							"SET @strSQL='' "+
            //							"DECLARE NewsTypeID CURSOR FOR  "+
            //							"SELECT DISTINCT n.TypeID FROM OA_NEWS n,OA_NEWS_TYPE nt WHERE n.TypeId=nt.TypeId and pubdate<=sysdate and nt.IsInner=1  "+
            //							"OPEN NewsTypeID  "+
            //							"FETCH NEXT FROM NewsTypeID INTO @TypeID "+
            //							"WHILE @@FETCH_STATUS = 0 "+
            //							"BEGIN "+
            //							"   SET @strSubSQL='SELECT TOP "+RecordCount.ToString()+" nt.TypeID,nt.TypeName,n.NewsId,n.FileName,n.SoftName,n.Title,n.Writer,n.DispFlag,n.PubDate,n.photo,n.InputDate,nt.isinner,nt.isouter '+ "+
            //							"      'FROM OA_NEWS n,OA_NEWS_TYPE nt '+  "+
            //							"      'WHERE n.TypeId=nt.TypeId AND pubdate<=sysdate AND nt.IsInner=1 AND nt.TypeID='+CONVERT(VARCHAR(8),@TypeID)  "+
            //							"   SET @strSQL = @strSQL+@strSubSQL + ' UNION '  "+
            //							"   FETCH NEXT FROM NewsTypeID INTO @TypeID "+
            //							"End "+
            //							"CLOSE NewsTypeID "+
            //							"DEALLOCATE NewsTypeID "+
            //							"IF (length(@strSQL)>7) SET @strSQL =LEFT(@strSQL,length(@strSQL)-6) "+
            //							"EXEC(@strSQL + ' ORDER BY TypeName,pubdate DESC') ";
            DataTable dt;
            OracleConnection cn = ConfigTool.GetConnection("SQLConnString");

            dt = OracleDbHelper.ExecuteDataTable(cn, CommandType.Text, strSQL);

            ConfigTool.CloseConnection(cn);

            return dt;
		}


		/// <summary>
		/// 获取首页信息目录
		/// </summary>
		public static DataTable GetShowNewsTitle(int intTypeID)
		{
            string strSQL = "SELECT nt.TypeName,n.NewsId,n.FileName,n.SoftName,n.Title,n.Writer,n.DispFlag,n.PubDate,n.photo,n.InOrgID,n.InDeptID,n.IsInner IsInnerN," +
                "n.InputDate,nt.isinner,nt.isouter" +
                " FROM OA_NEWS n,OA_NEWS_TYPE nt " +
                " WHERE ROWNUM<=20 and n.TypeId=nt.TypeId and pubdate<=sysdate and DispFlag=1 and n.TypeId=" + intTypeID.ToString() + " order by nt.TypeId,NewsId desc";
            DataTable dt;
            OracleConnection cn = ConfigTool.GetConnection("SQLConnString");

            dt = OracleDbHelper.ExecuteDataTable(cn, CommandType.Text, strSQL);

            ConfigTool.CloseConnection(cn);

            return dt;
		}

		/// <summary>
		/// 获取信息内容
		/// </summary>
		public static DataTable GetNewsList(long NewsId)
		{
			//string strSQL = "SELECT * FROM OA_NEWS WHERE NewsID="+NewsId;
            string strSQL = "SELECT isAlert, NewsId,Title,TypeId,Writer,to_char(InputDate,'yyyy-MM-dd HH24:mi:ss') InputDate,PubDate PubDate," +
                "to_char(OutDate,'yyyy-MM-dd') OutDate,InputUser,CheckUser,DispFlag,Photo,Content,FocusNews,IsBulletin,FileName,SoftName,InOrgID,InDeptID,IsInner,FlowID FROM OA_NEWS WHERE NewsID=" + NewsId;
			DataTable dt;
			OracleConnection cn = ConfigTool.GetConnection();
			dt = OracleDbHelper.ExecuteDataTable(cn,CommandType.Text,strSQL);
			ConfigTool.CloseConnection(cn);
			return dt;
		}
	}
}
