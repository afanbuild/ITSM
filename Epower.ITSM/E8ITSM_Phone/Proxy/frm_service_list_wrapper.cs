using System;
using System.Data;
using System.Configuration;
using System.Linq;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Xml.Linq;
using Epower.ITSM.SqlDAL;
using EpowerCom;

namespace E8ITSM_Phone.Proxy
{
    public class frm_service_list_wrapper
    {
        /// <summary>
        /// 递归的取服务目录层级
        /// </summary>
        /// <param name="lngUserId"></param>
        /// <returns></returns>
        public bool GetServiceListByUserId(long lngUserId,
            long lngServiceLevelId,
            DataTable _dt)
        {
            EA_ServicesTemplateDP ee = new EA_ServicesTemplateDP();
            string sWhere = String.Format(@" and ServiceLevelID = {0} ", lngServiceLevelId);
            DataTable dt = ee.GetDataTable(sWhere, "order by EA_ServicesTemplate.Templateid");

            Boolean isHave = false;

            if (dt.Rows.Count <= 0)
            {
                return isHave;
            }
            else
            {
                foreach (DataRow item in dt.Rows)
                {
                    if (long.Parse(item["TEMPLATEID"].ToString()) == -1)
                        continue;

                    lngServiceLevelId = long.Parse(item["TEMPLATEID"].ToString());
                    bool isSubDir = GetServiceListByUserId(lngUserId, lngServiceLevelId, _dt);

                    DataRow dr = _dt.NewRow();
                    for (int i = 0; i < _dt.Columns.Count; i++)
                    { dr[i] = item[i]; }

                    if (isSubDir)    // 若有子目录, 则不进行权限验证
                    {
                        _dt.Rows.Add(dr);
                    }
                    else    // 无子目录, 则验证权限
                    {
                        long lngOFlowModelId = long.Parse(item["OFlowModelID"].ToString());
                        bool isHaveRight = CanUseFModel(lngUserId, lngOFlowModelId);
                        if (isHaveRight)
                        {
                            isHave = true;
                            _dt.Rows.Add(dr);
                        }
                    }
                }
            }

            return isHave;
        }

        /// <summary>
        /// 构造一个服务目录的空表
        /// </summary>
        /// <returns></returns>
        public DataTable GetServiceListStruct()
        {
            EA_ServicesTemplateDP ee = new EA_ServicesTemplateDP();
            string sWhere = String.Format(@" and ServiceLevelID = -1 ");
            DataTable dt = ee.GetDataTable(sWhere, "order by EA_ServicesTemplate.Templateid");
            dt.Clear();
            return dt;
        }
        /// <summary>
        /// 判断用户是否可以启动对应的流程模型 两种情况不能启动流程模型： 1、流程模型不是启动状态,返回 -1 2、用户不是流程模型的启动人员 返回 -2
        /// </summary>
        /// <param name="lngUserId">用户编号</param>
        /// <param name="lngFlowModelID">流程模型编号</param>
        /// <returns>启动状态</returns>
        private bool CanUseFModel(long lngUserId, long lngFlowModelId)
        {
            long lngOFlowModelID = 0;
            long lngNewFlowModelID = 0;
            long lngUserID = lngUserId;

            lngOFlowModelID = FlowDP.GetOFlowModelID(lngFlowModelId);//原FlowModelID
            lngNewFlowModelID = FlowModel.GetLastVersionFlowModelID(lngOFlowModelID);//获取最新FlowModelID;

            int intCanStart = FlowModel.CanUseFlowModel(lngUserID, lngNewFlowModelID);

            return intCanStart == 0;
        }
    }
}
