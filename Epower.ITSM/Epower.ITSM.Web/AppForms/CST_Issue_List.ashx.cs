/*******************************************************************
 * 版权所有：
 * Description：ajax获取基本流程，快速登单模板方法
 * Create By  ：SuperMan
 * Create Date：2011-08-18
 * *****************************************************************/
using System;
using System.Collections;
using System.Data;
using System.Web;
using System.Web.Services;
using System.Web.Services.Protocols;
using System.Text;
using System.Data.OracleClient;
using Epower.DevBase.BaseTools;
using EpowerCom;
using Epower.ITSM.SqlDAL;

namespace Epower.ITSM.Web.AppForms
{
    /// <summary>
    /// $codebehindclassname$ 的摘要说明


    /// </summary>
    [WebService(Namespace = "http://tempuri.org/")]
    [WebServiceBinding(ConformsTo = WsiProfiles.BasicProfile1_1)]
    public class CST_Issue_List2 : IHttpHandler
    {
        #region ajax返回数据方法
        public void ProcessRequest(HttpContext context)
        {
            context.Response.ContentType = "text/plain";
            StringBuilder strReturn = new StringBuilder();

            if (context.Request["Type"] == "b")
            {
                #region 加载快速登单数据

                string strUserID = context.Request["UserId"].ToString();
                string strAppID = context.Request["appid"].ToString();


                DataTable dt = getFastBillList1(strUserID, strAppID);
                for (int i = 0; i < dt.Rows.Count; i++)
                {
                    strReturn.Append("<span><a title=" + (char)34 + dt.Rows[i]["TemplateName"].ToString() + (char)34 + " href=" + (char)34 + "#" + (char)34 + " onclick=" + (char)34 + "ShowFind('" + dt.Rows[i]["TemplateID"].ToString() + "')" + (char)34 + ">" + dt.Rows[i]["TemplateName"].ToString() + "</a></span><br/>");
                }

                #endregion
            }
            else if (context.Request["Type"].ToString().Contains("IssueCountA"))
            {
                #region 判断事件单基本流程模型数量,若为1，则返回流程模型ID

                DataTable dt = new DataTable();
                long lngUserID = Convert.ToInt64(context.Request["Type"].ToString().Split('|')[1].ToString());
                //dt = FlowModel.GetAllCanStartFlowModels(lngUserID, 1026).Tables[0];
                dt = ZHServiceDP.GetIssuesFlowModelManage(lngUserID);
                if (dt.Rows.Count == 1)
                {
                    //流程模型数为1
                    strReturn.Append(dt.Rows[0]["FlowModelID"].ToString());
                }
                #endregion
            }
            else if (context.Request["Type"].ToString().Contains("IssueCountB"))
            {
                #region 判断事件单快速登单模型数量,若为1，则返回流程模型ID
                string strUserID = context.Request["UserId"].ToString();
                string strAppID = context.Request["Type"].ToString().Split('|')[1].ToString();

                DataTable dt = getFastBillList1(strUserID, strAppID);
                if (dt.Rows.Count == 1)
                {
                    //流程模型数为1
                    strReturn.Append(dt.Rows[0]["TemplateID"].ToString());
                }
                #endregion
            }
            else if (context.Request["Type"].ToString().Contains("IssueB"))  //重复事件
            {
                #region 事件单基本流程数据

                DataSet ds;
                long lngUserID = Convert.ToInt64(context.Request["Type"].ToString().Split('|')[1].ToString());
                //ds = FlowModel.GetAllCanStartFlowModels(lngUserID, 1026);
                //for (int m = 0; m < ds.Tables[0].Rows.Count; m++)
                //{
                //    strReturn.Append("<span><a title=" + (char)34 + ds.Tables[0].Rows[m]["FlowName"].ToString() + (char)34 + " onclick=\"AddNewFlowMerge(" + ds.Tables[0].Rows[m]["FlowModelID"].ToString() + ")\"  href=\"#\">" + ds.Tables[0].Rows[m]["FlowName"].ToString() + "</a></span><br/>");
                //}
                DataTable dt = ZHServiceDP.GetIssuesFlowModelManage(lngUserID);

                for (int m = 0; m < dt.Rows.Count; m++)
                {
                    strReturn.Append("<span><a title=" + (char)34 + dt.Rows[m]["FlowName"].ToString() + (char)34 + " onclick=\"AddNewFlow(" + dt.Rows[m]["FlowModelID"].ToString() + ")\"  href=\"#\">" + dt.Rows[m]["FlowName"].ToString() + "</a></span><br/>");
                }

                #endregion
            }
            else if (context.Request["Type"].ToString().Contains("ProblemA"))
            {
                #region 问题单基本流程数据

                DataSet ds;
                long lngUserID = Convert.ToInt64(context.Request["Type"].ToString().Split('|')[1].ToString());
                ds = FlowModel.GetAllCanStartFlowModels(lngUserID, 210);
                for (int m = 0; m < ds.Tables[0].Rows.Count; m++)
                {
                    strReturn.Append("<span><a title=" + (char)34 + ds.Tables[0].Rows[m]["FlowName"].ToString() + (char)34 + " onclick=\"AddNewFlow(" + ds.Tables[0].Rows[m]["FlowModelID"].ToString() + ")\"  href=\"#\">" + ds.Tables[0].Rows[m]["FlowName"].ToString() + "</a></span><br/>");
                }
                #endregion
            }
            else if (context.Request["Type"].ToString().Contains("ProblemB"))   //问题单合并
            {
                #region 问题单基本流程数据

                DataSet ds;
                long lngUserID = Convert.ToInt64(context.Request["Type"].ToString().Split('|')[1].ToString());
                ds = FlowModel.GetAllCanStartFlowModels(lngUserID, 210);
                for (int m = 0; m < ds.Tables[0].Rows.Count; m++)
                {
                    strReturn.Append("<span><a title=" + (char)34 + ds.Tables[0].Rows[m]["FlowName"].ToString() + (char)34 + " onclick=\"AddNewFlowMerge(" + ds.Tables[0].Rows[m]["FlowModelID"].ToString() + ")\"  href=\"#\">" + ds.Tables[0].Rows[m]["FlowName"].ToString() + "</a></span><br/>");
                }
                #endregion
            }
            else if (context.Request["Type"].ToString().Contains("ProblemCountA"))
            {
                #region 计算问题单流程模型个数，若为1，则返回流程模型ID

                DataTable dt = new DataTable();
                long lngUserID = Convert.ToInt64(context.Request["Type"].ToString().Split('|')[1].ToString());
                dt = FlowModel.GetAllCanStartFlowModels(lngUserID, 210).Tables[0];
                if (dt.Rows.Count == 1)
                {
                    //流程模型数为1
                    strReturn.Append(dt.Rows[0]["FlowModelID"].ToString());
                }
                #endregion
            }
            else if (context.Request["Type"].ToString().Contains("ChangeA"))
            {
                #region 变更单基本流程数据



                DataSet ds;
                long lngUserID = Convert.ToInt64(context.Request["Type"].ToString().Split('|')[1].ToString());
                ds = FlowModel.GetAllCanStartFlowModels(lngUserID, 420);
                for (int m = 0; m < ds.Tables[0].Rows.Count; m++)
                {
                    strReturn.Append("<span><a title=" + (char)34 + ds.Tables[0].Rows[m]["FlowName"].ToString() + (char)34 + " onclick=\"AddNewFlow(" + ds.Tables[0].Rows[m]["FlowModelID"].ToString() + ")\"  href=\"#\">" + ds.Tables[0].Rows[m]["FlowName"].ToString() + "</a></span><br/>");
                }
                #endregion
            }
            else if (context.Request["Type"].ToString().Contains("ChangeCountA"))
            {
                #region 计算变更单流程模型个数，若为1，则返回流程模型ID

                DataTable dt = new DataTable();
                long lngUserID = Convert.ToInt64(context.Request["Type"].ToString().Split('|')[1].ToString());
                dt = FlowModel.GetAllCanStartFlowModels(lngUserID, 420).Tables[0];
                if (dt.Rows.Count == 1)
                {
                    //流程模型数为1
                    strReturn.Append(dt.Rows[0]["FlowModelID"].ToString());
                }
                #endregion
            }
            else if (context.Request["Type"].ToString().Contains("ReqDemandA"))    // 需求单
            {
                #region 需求单基本流程数据

                DataSet ds;
                long lngUserID = Convert.ToInt64(context.Request["Type"].ToString().Split('|')[1].ToString());
                ds = FlowModel.GetAllCanStartFlowModels(lngUserID, 1062);
                for (int m = 0; m < ds.Tables[0].Rows.Count; m++)
                {
                    strReturn.Append("<span><a title=" + (char)34 + ds.Tables[0].Rows[m]["FlowName"].ToString() + (char)34 + " onclick=\"AddNewFlow(" + ds.Tables[0].Rows[m]["FlowModelID"].ToString() + ")\"  href=\"#\">" + ds.Tables[0].Rows[m]["FlowName"].ToString() + "</a></span><br/>");
                }                
                #endregion
            }
            else if (context.Request["Type"].ToString().Contains("ReqDemandCountA"))    // 需求单个数
            {
                #region 计算需求单流程模型个数，若为1，则返回流程模型ID

                DataTable dt = new DataTable();
                long lngUserID = Convert.ToInt64(context.Request["Type"].ToString().Split('|')[1].ToString());
                dt = ZHServiceDP.GetIssuesFlowModelManage(lngUserID, 1062, 9711);
                if (dt.Rows.Count == 1)
                {
                    //流程模型数为1
                    strReturn.Append(dt.Rows[0]["FlowModelID"].ToString());
                }

                #endregion
            }
            else if (context.Request["Type"].ToString().Contains("ReqDemandCountB"))    // 需求快速登单
            {
                #region 判断需求快速登单模型数量,若为1，则返回流程模型ID
                string strUserID = context.Request["Type"].ToString().Split('|')[1].ToString();
                string strAppID = "1062";

                DataTable dt = getFastBillList1(strUserID, strAppID);
                if (dt.Rows.Count == 1)
                {
                    //流程模型数为1
                    strReturn.Append(dt.Rows[0]["TemplateID"].ToString());
                }
                #endregion
            }
            else if (context.Request["Type"].ToString().Contains("ReqDemandB"))
            {
                #region 加载快速登单数据

                string strUserID = context.Request["Type"].ToString().Split('|')[1].ToString();
                string strAppID = "1062";

                DataTable dt = getFastBillList1(strUserID, strAppID);
                for (int i = 0; i < dt.Rows.Count; i++)
                {
                    strReturn.Append("<span><a title=" + (char)34 + dt.Rows[i]["TemplateName"].ToString() + (char)34 + " href=" + (char)34 + "#" + (char)34 + " onclick=" + (char)34 + "ShowFind('" + dt.Rows[i]["TemplateID"].ToString() + "')" + (char)34 + ">" + dt.Rows[i]["TemplateName"].ToString() + "</a></span><br/>");
                }

                #endregion
            }
            else
            {
                #region 加载事件单基本流程数据




                DataSet ds;
                long lngUserID = Convert.ToInt64(context.Request["Type"].ToString().Split('|')[1].ToString());
                //ds = FlowModel.GetAllCanStartFlowModels(lngUserID, 1026);                
                //for (int m = 0; m < ds.Tables[0].Rows.Count; m++)
                //{
                //    strReturn.Append("<span><a title=" + (char)34 + ds.Tables[0].Rows[m]["FlowName"].ToString() + (char)34 + " onclick=\"AddNewFlow(" + ds.Tables[0].Rows[m]["FlowModelID"].ToString() + ")\"  href=\"#\">" + ds.Tables[0].Rows[m]["FlowName"].ToString() + "</a></span><br/>");
                //}
                DataTable dt = ZHServiceDP.GetIssuesFlowModelManage(lngUserID);

                for (int m = 0; m < dt.Rows.Count; m++)
                {
                    strReturn.Append("<span><a title=" + (char)34 + dt.Rows[m]["FlowName"].ToString() + (char)34 + " onclick=\"AddNewFlow(" + dt.Rows[m]["FlowModelID"].ToString() + ")\"  href=\"#\">" + dt.Rows[m]["FlowName"].ToString() + "</a></span><br/>");
                }

                #endregion
            }

            context.Response.Write(strReturn.ToString());
        }

        public bool IsReusable
        {
            get
            {
                return false;
            }
        }
        #endregion

        #region 加载快速登单模板


        /// <summary>
        /// 加载快速登单模板

        /// </summary>
        /// <returns></returns>
        private DataTable getFastBillList()
        {
            string SQLstr = @"select * from EA_ShortCutTemplate ";

            OracleConnection cn = new OracleConnection(System.Configuration.ConfigurationSettings.AppSettings["SQLConnString"]);
            DataTable dt = OracleDbHelper.ExecuteDataTable(cn, CommandType.Text, SQLstr);
            if (cn.State == ConnectionState.Open)
            {
                cn.Close();
            }
            return dt;
        }

        #endregion

        #region 加载快速登单模板
        /// <summary>
        /// 加载快速登单模板
        /// </summary>
        /// <returns></returns>
        private DataTable getFastBillList1(string userID,string strAppID)
        {
            string SQLstr = @"select * from EA_ShortCutTemplate WHERE Owner=0 and AppID=" + strAppID;
            long lngUserID = long.Parse(userID);
            OracleConnection cn = new OracleConnection(System.Configuration.ConfigurationSettings.AppSettings["SQLConnString"]);
            DataTable dt = OracleDbHelper.ExecuteDataTable(cn, CommandType.Text, SQLstr);
            long lngFlowModelID = 0;
            long lngOFlowModelID = 0;
            for (int i = 0; i < dt.Rows.Count; i++)
            {
                DataRow row = dt.Rows[i];
                row.BeginEdit();
                lngFlowModelID = long.Parse(row["OFlowModelID"].ToString());//FlowModelID
                lngOFlowModelID = FlowDP.GetOFlowModelID(lngFlowModelID);//原FlowModelID
                long lngNewFlowModelID = FlowModel.GetLastVersionFlowModelID(lngOFlowModelID);//获取最新FlowModelID;

                int intCanStart = FlowModel.CanUseFlowModel(lngUserID, lngNewFlowModelID);
                if (intCanStart != 0)
                {
                    dt.Rows[i].Delete();
                }
                else
                {
                    row["OFlowModelID"] = lngNewFlowModelID;
                    row.EndEdit();
                }
            }
            dt.AcceptChanges();

            if (cn.State == ConnectionState.Open)
            {
                cn.Close();
            }
            return dt;
        }

        #endregion
    }
}
