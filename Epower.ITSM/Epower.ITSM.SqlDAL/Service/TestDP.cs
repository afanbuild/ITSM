using System;
using System.Collections.Generic;
using System.Text;
using EpowerGlobal;
using MyComponent;
using System.Data;

namespace Epower.ITSM.SqlDAL
{
    public class TestDP
    {
        /// <summary>
        /// 获取用户未接收列表
        /// </summary>
        /// <param name="lngUserID"></param>
        /// <param name="pageSize"></param>
        /// <returns></returns>
        public static DataSet GetReceiveMessageList(long lngUserID, int pageSize)
        {
            string strSQL = "";
            strSQL = @"SELECT f.* FROM 
                       (
                        SELECT d.ID,a.MessageID,a.IsRead,a.Important,a.FActors,a.ReceiveTime,a.actortype,b.Subject,b.Attachment,b.Name as FlowName,c.AppID,c.AppName as AppName,datediff('Minute',sysdate,nvl(a.expected,sysdate)) as DiffMinute,b.FlowID " +
                        " FROM Es_Message a,Es_Flow b,Es_App c,Es_ReceiveList d " +
                        " WHERE d.MessageID = a.MessageID AND a.FlowID = b.FlowID AND b.AppID = c.AppID " +
                        " AND d.ReceiveID = " + lngUserID.ToString() +
                        " AND a.Deleted =" + (int)e_Deleted.eNormal +
                        " AND a.ReceiverID = 0 " +
                        " ORDER BY a.MessageID DESC" +
                        ") f" +
                        " WHERE  ROWNUM<=" + pageSize.ToString();
            return MyDataBase.QueryDataSetByStr(strSQL);

        }

    }
}
