using System;
using System.Collections.Generic;
using System.Text;
using EpowerGlobal;
using System.Data;

namespace Epower.ITSM.SqlDAL.Mobile
{
    public class HasProcessedDP
    {
        #region property
        /// <summary>
        /// 用户编号
        /// </summary>
        private int _userId;
        /// <summary>
        /// 消息状态
        /// </summary>
        private e_MessageStatus _messageStatus;
        /// <summary>
        /// 是否删除
        /// </summary>
        private e_Deleted _isDeleted;
        /// <summary>
        /// 已办事项单集合
        /// </summary>
        private DataTable _dtRet;
        /// <summary>
        /// SQL Script: 查询已办事件单.
        /// SQL Params: status, deleted, receiverid.
        /// </summary>
        private const string _QUERY_ISSUE_LIST = @"SELECT (E.buildcode || E.Serviceno) as 
                              NumberNo,e.flowmodelid,E.ServiceKind,E.ServiceType,E.regusername,D.subject,A.FlowID,
                              A.FActors,E.ServiceLevel,
                              A.MessageID,A.ReceiveTime,A.senderusername,A.sendernodename,
                              A.IsRead,A.Important,D.Attachment,c.appid,c.AppName,
                              datediff('Minute',sysdate,nvl(a.expected,sysdate)) as DiffMinute,
                              d.Name as FlowName,A.actortype ,E.FinishedTime,E.Content
                              FROM cst_issues E,Es_Flow D, V_Es_Message A,Es_App c
                              WHERE E.FlowID=A.FlowID AND D.FlowID=A.FlowID AND E.FlowID=A.FlowID 
                              AND D.Appid=c.AppID 
                              AND A.status={0} AND A.Deleted = {1}
                              AND D.AppId = 1026 AND A.ReceiverId={2}
                              order by E.FlowId desc ";
        /// <summary>
        /// SQL Script: 查询已办变更单.
        /// SQL Params: status, deleted, receiverid.
        /// </summary>
        private const string _QUERY_CHANGE_LIST = @"SELECT e.flowmodelid,E.changeno NumberNo,D.subject,A.FlowID,
                        A.FActors,E.LevelName,
                        A.MessageID,A.ReceiveTime,A.senderusername,A.sendernodename,
                        A.IsRead,A.Important,D.Attachment,c.appid,c.AppName,
                        datediff('Minute',sysdate,nvl(a.expected,sysdate)) as DiffMinute,d.Name as 
                        FlowName,A.actortype
                        FROM equ_changeservice E,Es_Flow D, V_Es_Message A,Es_App c
   	                    WHERE E.FlowID=A.FlowID AND D.FlowID=A.FlowID AND E.FlowID=A.FlowID 
                        AND D.Appid=c.AppID
                        AND A.status= {0}
                        AND A.Deleted = {1}
                        AND D.AppId = 420
                        AND A.ReceiverId= {2}
                        order by E.FlowId desc";
        #endregion

        /// <summary>
        /// 初始化取已办事项的基础参数
        /// </summary>
        /// <param name="userId"></param>
        /// <param name="messageStatus"></param>
        /// <param name="isDeleted"></param>
        public HasProcessedDP(int userId,
            e_MessageStatus messageStatus,
            e_Deleted isDeleted)
        {
            this._userId = userId;
            this._messageStatus = messageStatus;
            this._isDeleted = isDeleted;

            /*
             * 汇集事件单, 变更单的资料, 作为返回数据.
             * 目前有: RowNO, FlowModelId, MessageId, Subject, ReceiveTime             
             */
            _dtRet = new DataTable();
            _dtRet.Columns.Add(new DataColumn("RowNO", typeof(string)));
            _dtRet.Columns.Add(new DataColumn("RowType", typeof(string)));
            _dtRet.Columns.Add(new DataColumn("FlowModelId", typeof(int)));
            _dtRet.Columns.Add(new DataColumn("MessageId", typeof(int)));
            _dtRet.Columns.Add(new DataColumn("Subject", typeof(string)));
            _dtRet.Columns.Add(new DataColumn("ReceiveTime", typeof(DateTime)));
            _dtRet.Columns.Add(new DataColumn("FlowID", typeof(int)));   

            _dtRet.PrimaryKey = new DataColumn[] { _dtRet.Columns["RowNO"] };
        }       

        /// <summary>
        /// 获取已办事项列表
        /// </summary>
        /// <returns></returns>
        public DataTable GetMsgList()
        {
            /* 方法被重复调用, 清理掉上次查询数据 */
            _dtRet.Rows.Clear();

            FetchIssueList();
            FetchChangeList();

            return _dtRet;
        }

        /// <summary>
        /// 取已办事件单
        /// </summary>
        private void FetchIssueList()
        {
            /* 取已办事件单 */
            String strSQLText = String.Format(_QUERY_ISSUE_LIST,
                (int)this._messageStatus, (int)this._isDeleted, this._userId);

            DataTable dtIssue = CommonDP.ExcuteSqlTable(strSQLText);

            /* 取已办事件单, 并过滤重复单. */
            foreach (DataRow item in dtIssue.Rows)
            {
                String __numberNo = item["NumberNo"].ToString().Trim();
                Boolean isHave = _dtRet.Rows.Find(__numberNo) != null;

                if (isHave)
                    continue;

                DataRow dr = _dtRet.NewRow();
                dr["RowNO"] = __numberNo;
                dr["FlowModelId"] = item["flowmodelid"];
                dr["MessageID"] = item["MessageID"];
                dr["Subject"] = item["subject"];
                dr["ReceiveTime"] = item["ReceiveTime"];
                dr["RowType"] = "Issue";
                dr["FlowID"] = item["FLOWID"];

                _dtRet.Rows.Add(dr);
            }
        }
        /// <summary>
        /// 取已办变更单
        /// </summary>
        private void FetchChangeList()
        {
            /* 取已办变更单 */
            String strSQLText = String.Format(_QUERY_CHANGE_LIST,
                (int)this._messageStatus, (int)this._isDeleted, this._userId);

            DataTable dtChange = CommonDP.ExcuteSqlTable(strSQLText);

            foreach (DataRow item in dtChange.Rows)
            {
                String __numberNo = item["NumberNo"].ToString().Trim();
                Boolean isHave = _dtRet.Rows.Find(__numberNo) != null;

                if (isHave)
                    continue;

                DataRow dr = _dtRet.NewRow();
                dr["RowNO"] = __numberNo;
                dr["FlowModelId"] = item["flowmodelid"];
                dr["MessageID"] = item["MessageID"];
                dr["Subject"] = item["subject"];
                dr["ReceiveTime"] = item["ReceiveTime"];
                dr["RowType"] = "Change";
                dr["FlowID"] = item["FLOWID"];

                _dtRet.Rows.Add(dr);
            }
        }
    }
}
