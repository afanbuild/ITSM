using System;
using System.Data;
using System.Data.OracleClient;
using MyComponent;
using System.Xml;
using System.IO;
using System.Collections;
using System.Collections.Generic;
 
using IappDataProcess;
using EpowerGlobal;
using EpowerCom;
using Epower.ITSM.SqlDAL;

namespace appDataProcess.ActorExtends
{
	/// <summary>
	/// 地面局人员
	/// </summary>
	public class ActExtKZPeople:IActorExtend
	{
        public ActExtKZPeople()
		{
			//
			// TODO: 在此处添加构造函数逻辑
			//
		}

        /// <summary>
        /// 获取各个接口的具体实现
        /// </summary>
        /// <param name="lngStarterID">起草人编号</param>
        /// <param name="lngSenderID">发送人编号(当前用户编号)</param>
        /// <param name="lngFlowID"></param>
        /// <param name="lngFlowModelID">流程模型编号</param>
        /// <param name="lngNodeModelID">环节模型编号</param>
        /// <param name="strFormValues"></param>
        /// <returns></returns>
        public override BaseCollection GetActors(long lngStarterID, long lngSenderID, long lngFlowID, long lngFlowModelID, long lngNodeModelID, string strFormValues)
        {
            BaseCollection bc = new BaseCollection();
        

            FieldValues fv = new FieldValues(strFormValues);
            Cst_RecommendRuleDP RuleDP =new Cst_RecommendRuleDP();
            long CustID=0;
            string EquName = string.Empty;
            long ServiceTypeID=0;
            long ServiceLevelID=0;
            string stCustName = "";
            string stMastCustName = "";

            if (fv.GetFieldValue("CustID") != null && fv.GetFieldValue("CustID").Value != "")
            {
                CustID = long.Parse(fv.GetFieldValue("CustID").Value);
            }
            if (fv.GetFieldValue("EquipmentName") != null && fv.GetFieldValue("EquipmentName").Value != "")
            {
                EquName = fv.GetFieldValue("EquipmentName").Value.ToString();
            }
            if (fv.GetFieldValue("ServiceTypeID") != null && fv.GetFieldValue("ServiceTypeID").Value != "")
            {
                ServiceTypeID = long.Parse(fv.GetFieldValue("ServiceTypeID").Value);   
            }
            if (fv.GetFieldValue("ServiceLevelID") != null && fv.GetFieldValue("ServiceLevelID").Value != "")
            {
                ServiceLevelID = long.Parse(fv.GetFieldValue("ServiceLevelID").Value);
            }
            if (fv.GetFieldValue("CustName") != null && fv.GetFieldValue("CustName").Value != "")
            {
                stCustName = fv.GetFieldValue("CustName").Value;
            }
            if (fv.GetFieldValue("MastCust") != null && fv.GetFieldValue("MastCust").Value != "")
            {
                stMastCustName = fv.GetFieldValue("MastCust").Value;
            }

            DataTable dt = RuleDP.getUserProject(CustID, EquName, ServiceTypeID, ServiceLevelID,stCustName,stMastCustName);



            List<GS_Engineer_SchedulesDP> issuesEngineerList = new GS_Engineer_SchedulesDP().GetCurrentIssues();

            if (issuesEngineerList == null && issuesEngineerList.Count == 0)
            {
                if (dt.Rows.Count > 0)
                {
                    foreach (DataRow dr in dt.Rows)
                    {
                        bc.Add(int.Parse(dr["userid"].ToString()), dr["username"].ToString());
                    }
                }
            }
            else
            {
                if (dt.Rows.Count > 0)
                {
                    foreach (DataRow dr in dt.Rows)
                    {
                        Epower.DevBase.BaseTools.E8Logger.Info("engineer id:" + dr["userid"].ToString());
                        GS_Engineer_SchedulesDP issues = issuesEngineerList.Find(p => { return p.ENGINEERID == long.Parse(dr["userid"].ToString()); });

                        if (issues != null)
                        {
                            bc.Add(int.Parse(dr["userid"].ToString()), dr["username"].ToString());
                        }
                    }
                }
            
            }
            

            return bc;
        }

        private void test()
        {
              
        }
	}
}
