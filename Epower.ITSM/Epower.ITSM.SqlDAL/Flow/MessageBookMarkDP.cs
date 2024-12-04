using System;
using System.Data;
using System.Data.OracleClient;
using Epower.DevBase.BaseTools;
using Epower.ITSM.Base;
using EpowerGlobal;
using System.Xml;
using System.IO;

namespace Epower.ITSM.SqlDAL
{
	/// <summary>
	/// MessageBookMarkDP 的摘要说明。
	/// </summary>
	public class MessageBookMarkDP
	{
		public MessageBookMarkDP()
		{
			//
			// TODO: 在此处添加构造函数逻辑
			//
		}


		/// <summary>
		/// 获取事项查询时的列表
		/// </summary>
		/// <param name="lngUserID"></param>
		/// <param name="iBMK"></param>
		/// <returns></returns>
		public static DataTable GetMessageBookMarkList(long lngUserID,eOA_ListBookMark iBMK,string sOrder, int pagesize, int pageindex, ref int rowcount)
		{
			string strSQL = " ";
            System.Text.StringBuilder sbSql = new System.Text.StringBuilder();
			switch(iBMK)
			{
				case eOA_ListBookMark.elbmOverTime:
                    strSQL = @" status = 10 AND 
							receivetime BETWEEN dateadd('month',-1,sysdate) AND sysdate
							AND ReceiverID=" + lngUserID.ToString() + @" AND datediff('Minute',recentprocesstime,nvl(expected,recentprocesstime)) < 0 ";
					break;
				case eOA_ListBookMark.elbmInTime:
                    sbSql.Append("status = 10");
                    sbSql.Append(" AND receivetime BETWEEN dateadd('month',-1,sysdate) AND sysdate");
                    sbSql.AppendFormat(" AND ReceiverID={0} ", lngUserID);
                    sbSql.Append(" AND recentprocesstime>= nvl(expected ,recentprocesstime)");
                    sbSql.Append(" AND recentprocesstime is not null ");
                    //sbSql.AppendFormat(" AND datediff('Minute',recentprocesstime,nvl(expected,recentprocesstime)) >= 0");
//                    strSQL = @" status = 10 AND 
//							receivetime BETWEEN dateadd('month',-1,sysdate) AND sysdate
//							AND ReceiverID=" + lngUserID.ToString() + @" AND datediff('Minute',recentprocesstime,nvl(expected,recentprocesstime)) >= 0 ";
                    strSQL = sbSql.ToString();
					break;
				case eOA_ListBookMark.elbmMaster:
                    strSQL = @" actortype = " + ((int)e_ActorClass.fmMasterActor).ToString() + @" AND 
							receivetime BETWEEN dateadd('month',-1,sysdate) AND sysdate
							AND ReceiverID=" + lngUserID.ToString() ;
					break;
				case eOA_ListBookMark.elbmAssist:
                    strSQL = @" actortype = " + ((int)e_ActorClass.fmAssistActor).ToString() + @" AND 
							receivetime BETWEEN dateadd('month',-1,sysdate) AND sysdate
							AND ReceiverID=" + lngUserID.ToString() ;
					break;
				case eOA_ListBookMark.elbmReader:
                    strSQL = @" actortype = " + ((int)e_ActorClass.fmReaderActor).ToString() + @" AND 
							receivetime BETWEEN dateadd('month',-1,sysdate) AND sysdate
							AND ReceiverID=" + lngUserID.ToString();
					break;
				case eOA_ListBookMark.elbmInflux:
                    strSQL = @" actortype = " + ((int)e_ActorClass.fmInfluxActor).ToString() + @" AND 
							receivetime BETWEEN dateadd('month',-1,sysdate) AND sysdate
							AND ReceiverID=" + lngUserID.ToString();
					break;
				default:
					strSQL = " 1=1 ";
					break;

			}

            DataTable dt = null;
            OracleConnection cn = ConfigTool.GetConnection();
            dt = OracleDbHelper.ExecuteDataTable(cn, "V_FlowQueryAll", "*", sOrder, pagesize, pageindex, strSQL, ref rowcount);
            ConfigTool.CloseConnection(cn);

            return dt;
		}

        /// <summary>
        /// 根据ＸＭＬ串条件获取消息集合
        /// </summary>
        /// <param name="strXmlcond"></param>
        /// <param name="sOrder"></param>
        /// <param name="pagesize"></param>
        /// <param name="pageindex"></param>
        /// <param name="rowcount"></param>
        /// <returns></returns>
        public static DataTable GetMessagesForCond(string strXmlcond, string sOrder, int pagesize, int pageindex, ref int rowcount)
        {
            #region 获取查询参数的值
            string strSQL = "";
            string strTmp = "";

            string strStatus = "";
            string strSubject = "";
            string strAppID = "";

            string strMessageBegin = "";
            string strMessageEnd = "";
            string strProcessBegin = "";
            string strProcessEnd = "";


            long lngUserDeptID = 0;
            long lngUserID = 0;

            XmlTextReader tr = new XmlTextReader(new StringReader(strXmlcond));
            while (tr.Read())
            {
                if (tr.Name == "Field" && tr.NodeType == XmlNodeType.Element)
                {
                    strTmp = tr.GetAttribute("Value").Trim();
                    switch (tr.GetAttribute("FieldName"))
                    {
                        case "Status":
                            strStatus = strTmp;
                            break;
                        case "Subject":
                            strSubject = strTmp;
                            break;
                        case "AppID":
                            strAppID = strTmp;
                            break;

                        case "MessageBegin":
                            strMessageBegin = strTmp;
                            break;
                        case "MessageEnd":
                            strMessageEnd = strTmp;
                            break;
                        case "ProcessBegin":
                            strProcessBegin = strTmp;
                            break;
                        case "ProcessEnd":
                            strProcessEnd = strTmp;
                            break;
                        case "UserID":
                            lngUserID = long.Parse(strTmp);
                            break;
                        case "UserDeptID":
                            lngUserDeptID = long.Parse(strTmp);
                            break;
                        default:
                            break;
                    }
                }
            }
            tr.Close();
            #endregion
            string strWhere = "";
            string strInnerWhere = "";
            string strMessageTable = "";
            string strOuterWhere = "";

            if (strStatus != "-1" && strStatus != "")
            {
                strWhere += " AND status = " + strStatus;
            }
            if (strSubject.Length != 0)
            {
                strWhere += " AND subject like " + StringTool.SqlQ("%" + strSubject + "%");
                strOuterWhere += " AND subject like " + StringTool.SqlQ("%" + strSubject + "%");
            }

            if (strAppID != "-1" && strAppID != "")
            {
                strWhere += " AND appid = " + strAppID;
                strOuterWhere += " AND appid = " + strAppID;
            }

            if (strMessageBegin != "")
            {
                strWhere += " AND receivetime >= to_date(" + StringTool.SqlQ(strMessageBegin) + ",'yyyy-MM-dd HH24:mi:ss')";
                strInnerWhere += " AND receivetime >= to_date(" + StringTool.SqlQ(strMessageBegin) + ",'yyyy-MM-dd HH24:mi:ss')";
            }

            if (strMessageEnd != "")
            {
                strWhere += " AND receivetime <= to_date(" + StringTool.SqlQ(strMessageEnd) + ",'yyyy-MM-dd HH24:mi:ss')";
                strInnerWhere += " AND receivetime <= to_date(" + StringTool.SqlQ(strMessageEnd) + ",'yyyy-MM-dd HH24:mi:ss')";
            }
            if (strProcessBegin != "")
            {
                strWhere += " AND recentprocesstime >= to_date(" + StringTool.SqlQ(strProcessBegin) + ",'yyyy-MM-dd HH24:mi:ss')";
                strInnerWhere += " AND recentprocesstime >= to_date(" + StringTool.SqlQ(strProcessBegin) + ",'yyyy-MM-dd HH24:mi:ss')";
            }

            if (strProcessEnd != "")
            {
                strWhere += " AND recentprocesstime <= to_date(" + StringTool.SqlQ(strProcessEnd) + ",'yyyy-MM-dd HH24:mi:ss')";
                strInnerWhere += " AND recentprocesstime <= to_date(" + StringTool.SqlQ(strProcessEnd) + ",'yyyy-MM-dd HH24:mi:ss')";
            }

            if (strStatus.Trim() == ((int)e_MessageStatus.emsHandle).ToString().Trim() || strStatus == ((int)e_MessageStatus.emsWaiting).ToString())
            {
                strMessageTable = " ";
            }
            else
            {
                if (strStatus == "-1" || strStatus == "")
                {
                    //全部消息　注：全部消息不查询出终止的消息
                    strMessageTable = " AND messageid IN " +
                        "(SELECT MAX(messageid) FROM es_message WHERE deleted = 0 " +
                        " AND (status = " + ((int)e_MessageStatus.emsHandle).ToString() +
                        " OR status = " + ((int)e_MessageStatus.emsWaiting).ToString() + ")" + strInnerWhere +
                        " GROUP BY flowid,receiverid    ";
                    strMessageTable += " UNION SELECT MAX(messageid) FROM es_message WHERE deleted = 0 " +
                        " AND status = " + ((int)e_MessageStatus.emsFinished).ToString() + strInnerWhere +
                        " GROUP BY flowid,receiverid )";
                }
                else
                {
                    //终止 或 正常结束 
                    strMessageTable = " AND messageid IN " +
                        "(SELECT MAX(messageid) FROM es_message WHERE deleted = 0 " +
                        " AND status = " + strStatus + strInnerWhere +
                        "GROUP BY flowid,receiverid)";
                }
            }
            strSQL = " ReceiverID=" + lngUserID.ToString() ;


            if (strStatus.Trim() == ((int)e_MessageStatus.emsHandle).ToString().Trim() || strStatus == ((int)e_MessageStatus.emsWaiting).ToString())
            {
                strSQL = strSQL + strWhere + strMessageTable;
            }
            else
            {
                //内部已经条件过滤，这里只需要加外部过滤条件
                strSQL = strSQL + strOuterWhere + strMessageTable;
            }
            OracleConnection cn = ConfigTool.GetConnection();
            DataTable dt = OracleDbHelper.ExecuteDataTable(cn, "V_FlowQueryAll", "distinct *", sOrder, pagesize, pageindex, strSQL, ref rowcount);
            ConfigTool.CloseConnection(cn);

            return dt;
        }

	}
}
