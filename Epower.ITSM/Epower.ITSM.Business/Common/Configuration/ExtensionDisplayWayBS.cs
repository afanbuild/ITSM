/*******************************************************************
 * 版权所有：深圳市非凡信息技术有限公司
 * 描述：流程自定义扩展项显示方式-业务逻辑代码
 * 
 * 
 * 创建人：孙绍棕
 * 创建日期：2013-12-04
 * *****************************************************************/

using System;
using System.Collections.Generic;
using System.Text;
using System.Data;
using Epower.ITSM.SqlDAL.Customer;
using Epower.DevBase.BaseTools;
using System.Web.UI.WebControls;
using Epower.ITSM.SqlDAL;
using System.Data.OracleClient;
using System.Xml;
using EpowerCom;

namespace Epower.ITSM.Business.Common.Configuration
{
    /// <summary>
    /// 流程自定义扩展项显示方式-业务逻辑
    /// </summary>
    public class ExtensionDisplayWayBS
    {
        /// <summary>
        /// 取扩展项显示方式的环节模型列表
        /// </summary>
        /// <param name="lngAppID">应用编号</param>
        /// <param name="lngFlowModelID">流程模型编号</param>
        /// <param name="lngNodeModelID">环节模型编号</param>
        /// <param name="strSearchKey">查询关键词</param>
        /// <returns></returns>
        public DataTable GetExNodeModelList(long lngAppID,
            long lngFlowModelID,
            long lngNodeModelID,
            String strSearchKey)
        {

            ClearDirtyData();


            StringBuilder sbSql = new StringBuilder();
            if (lngAppID > 0)
                sbSql.AppendFormat(" and appid = {0}", lngAppID);
            if (lngFlowModelID > 0)
                sbSql.AppendFormat(" and flowmodelid = {0}", lngFlowModelID);
            if (lngNodeModelID > 0)
                sbSql.AppendFormat(" and nodemodelid = {0}", lngNodeModelID);
            //if (!String.IsNullOrEmpty(strSearchKey))
            //    sbSql.AppendFormat(" and ( appname like {0} or flowmodelname like {0} or nodename like {0} ) ",
            //        StringTool.SqlQ("%" + strSearchKey + "%"));

            DataTable dtExNodeModelList = Br_ExtensionDisplayWayDP.GetDataTable(sbSql.ToString(), "");

            dtExNodeModelList.Columns.Add("appname", typeof(String));
            dtExNodeModelList.Columns.Add("flowmodelname", typeof(String));
            dtExNodeModelList.Columns.Add("nodename", typeof(String));

            for (int index = 0; index < dtExNodeModelList.Rows.Count; index++)
            {
                DataRow drExItem = dtExNodeModelList.Rows[index];
                long _lngFlowModelID = long.Parse(drExItem["flowmodelid"].ToString());
                long _lngNodeModelID = long.Parse(drExItem["nodemodelid"].ToString());

                long _lngAppID = FlowModel.GetFlowModelAppID(_lngFlowModelID);

                String strNodelName = Br_ExtensionDisplayWayDP.GetFlowModelName(_lngFlowModelID, _lngNodeModelID);

                String strFlowModelName = Br_ExtensionDisplayWayDP.GetFlowModelName(_lngFlowModelID);
                String strAppName = Br_ExtensionDisplayWayDP.GetAppName(_lngAppID);

                drExItem["appname"] = strAppName;
                drExItem["flowmodelname"] = strFlowModelName;
                drExItem["nodename"] = strNodelName;
            }

            return dtExNodeModelList;
        }

        /// <summary>
        /// 取扩展项显示方式的列表
        /// </summary>
        /// <param name="lngNodeModelID">环节模型编号</param>
        /// <returns></returns>
        public DataTable GetExDisplayWayList(long lngFlowModelID, long lngNodeModelID)
        {
            DataTable dtExDisplayWay2 = this.GetBlankExDisplayWayList(lngFlowModelID);    // 用于组装后返回
            DataTable dtExDisplayWay = Br_ExtensionDisplayWayDP.GetExDisplayWayList(lngNodeModelID, lngFlowModelID);

            dtExDisplayWay2.Columns.Add("displaystatus", typeof(int));

            for (int index = 0; index < dtExDisplayWay.Rows.Count; index++)
            {
                DataRow dr = dtExDisplayWay.Rows[index];
                //Int32 itemTypeIndex = Int32.Parse(dr["itemtype"].ToString());
                //String strItemTypeName = ExtensionItemBS.ConvertIndexToItemTypeName(itemTypeIndex);
                //dr["typename"] = strItemTypeName;

                long lngFieldID = long.Parse(dr["fieldid"].ToString());

                for (int indexj = 0; indexj < dtExDisplayWay2.Rows.Count; indexj++)
                {
                    DataRow dr2 = dtExDisplayWay2.Rows[indexj];
                    long _lngFieldID = long.Parse(dr2["fieldid"].ToString());

                    if (lngFieldID == _lngFieldID)
                    {
                        dr2["displaystatus"] = dr["displaystatus"];
                    }

                }
            }

            return dtExDisplayWay2;
        }


        /// <summary>
        /// 更新各扩展项显示方式所属的FieldID
        /// </summary>
        /// <param name="trans">事务对象</param>
        /// <param name="lngFlowModelID">流程模型编号</param>
        /// <param name="dtExItemList">扩展项列表</param>
        /// <returns></returns>
        public void UpdateExDisplayFieldID(OracleTransaction trans,
            long lngFlowModelID, DataTable dtExItemList)
        {
            foreach (DataRow dr in dtExItemList.Rows)
            {
                int ofieldid = int.Parse(dr["id"].ToString());
                int nfieldid = int.Parse(dr["nfieldid"].ToString());

                Br_ExtensionDisplayWayDP.UpdateFieldID(trans, lngFlowModelID, nfieldid, ofieldid);
            }
        }

        /// <summary>
        /// 取空的扩展项显示方式列表
        /// </summary>
        /// <param name="lngFlowModelID">流程模型编号</param>
        /// <returns></returns>
        public DataTable GetBlankExDisplayWayList(long lngFlowModelID)
        {
            ExtensionItemBS extensionItemBS = new ExtensionItemBS();
            DataTable dtExItemList = extensionItemBS.GetExItemListByFlowModelID(lngFlowModelID);

            dtExItemList.Columns["ID"].ColumnName = "FieldID";

            return dtExItemList;
        }

        /// <summary>
        /// 保存扩展项的显示方式列表
        /// </summary>
        /// <param name="lngFlowModelID">流程模型编号</param>
        /// <param name="lngNodeModelID">环节模型编号</param>
        /// <param name="dtExDisplayWay">扩展项显示方式列表</param>
        public void SaveExDisplayWayList(long lngFlowModelID,
            long lngNodeModelID,
            List<KeyValuePair<long, int>> listExDisplayWay)
        {
            OracleConnection conn = null;
            OracleTransaction trans = null;
            try
            {
                conn = ConfigTool.GetConnection();
                conn.Open();

                trans = conn.BeginTransaction();

                Br_ExtensionDisplayWayDP.DeleteNodeModel(trans, lngFlowModelID, lngNodeModelID);

                foreach (KeyValuePair<long, int> item in listExDisplayWay)
                {
                    Br_ExtensionDisplayWayDP.AddExDisplayWayList(trans,
                        lngFlowModelID,
                        lngNodeModelID,
                        item.Key/* => FieldID */,
                        item.Value/* => DisplayStatus */);
                }

                trans.Commit();

            }
            catch (Exception ex)
            {
                if (trans != null)
                    trans.Rollback();

                throw ex;
            }
            finally
            {
                if (conn != null)
                    ConfigTool.CloseConnection(conn);
            }
        }

        /// <summary>
        /// 删除扩展项显示方式所属的环节模型列表
        /// </summary>
        /// <param name="ListNodeModel"></param>
        public void DeleteExNodeModelList(List<KeyValuePair<long, long>> ListNodeModel)
        {
            bool deleted = true;
            OracleConnection conn = null;
            OracleTransaction trans = null;
            try
            {
                conn = ConfigTool.GetConnection();
                conn.Open();

                trans = conn.BeginTransaction();

                foreach (KeyValuePair<long, long> kv in ListNodeModel)
                {
                    Br_ExtensionDisplayWayDP.DeleteNodeModel(trans, kv.Key, kv.Value);
                }

                trans.Commit();
            }
            catch (Exception ex)
            {
                if (trans != null)
                    trans.Rollback();

                throw ex;
            }
            finally
            {
                if (conn != null)
                    ConfigTool.CloseConnection(conn);
            }
        }



        /// <summary>
        /// 删除拥有扩展方式的流程模型
        /// </summary>
        /// <param name="trans">事务对象</param>
        /// <param name="lngFlowModelID">流程模型编号</param>
        public void DeleteExFlowModel(OracleTransaction trans, long lngFlowModelID)
        {
            OracleDbHelper.ExecuteNonQuery(trans, String.Format("delete from br_extensiondisplaystatus where flowmodelid = {0}", lngFlowModelID));
        }


        /// <summary>
        /// 清理脏数据
        /// </summary>
        public void ClearDirtyData()
        {
            DataTable dtFlowModel = Br_ExtensionDisplayWayDP.GetFlowModelList();

            foreach (DataRow row in dtFlowModel.Rows)
            {
                long lngOFlowModelID = long.Parse(row["flowmodelid"].ToString());

                long lngFlowModelID = FlowModel.GetLastVersionFlowModelID(lngOFlowModelID);
                Br_ExtensionDisplayWayDP.DeleteNodeModelDirtyData(lngFlowModelID, lngOFlowModelID);
            }
        }

        /// <summary>
        /// 加载环节模型列表到DropDownList
        /// </summary>
        /// <param name="ddlObj">下拉框对象</param>
        /// <param name="lngAppID">应用编号</param>
        /// <param name="lngFlowModelID">流程模型编号</param>
        public static void LoadNodeModelListToDropDownList(DropDownList ddlObj, long lngAppID, long lngFlowModelID)
        {
            MailAndMessageRuleDP mRulDP = new MailAndMessageRuleDP();

            DataTable dt = mRulDP.GetNodeName(lngAppID, lngFlowModelID);
            DataView dv = dt.DefaultView;

            ddlObj.DataSource = dv;
            ddlObj.DataTextField = "nodename";
            ddlObj.DataValueField = "NodeModelID";
            ddlObj.DataBind();
            ddlObj.Items.Insert(0, new ListItem("", ""));//add            
        }



        /// <summary>
        /// 保存全部环节模型的可见可编辑状态
        /// </summary>
        /// <param name="lngFlowModelID">流程模型编号</param>
        /// <param name="listExDisplayWay">扩展项的可见可编辑状态</param>        
        public void SaveAllNodeModelDisplayStatus(
            long lngFlowModelID,
            List<KeyValuePair<long, int>> listExDisplayWay)
        {

            /*
             * 请注意:
             * 
             * 可见可编辑状态保存时 是以 oFlowModelID 作为流程编号的！
             * 
             * **/

            lngFlowModelID = FlowModel.GetLastVersionFlowModelID(lngFlowModelID);

            DataTable dtNodeModel = CommonDP.ExcuteSqlTable(String.Format(@"select * from es_nodemodel where flowmodelid = {0} and nodemodelid <> 1", lngFlowModelID));

            lngFlowModelID = FlowDP.GetOFlowModelID(lngFlowModelID);

            foreach (DataRow item in dtNodeModel.Rows)
            {
                long lngNodeModelID = long.Parse(item["nodemodelid"].ToString());
                this.SaveExDisplayWayList(lngFlowModelID, lngNodeModelID, listExDisplayWay);
            }

        }

    }
}
