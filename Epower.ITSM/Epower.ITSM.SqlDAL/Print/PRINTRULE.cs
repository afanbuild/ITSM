
/************************************************************************************************************************************************
 *   创建人：yanghw
 * 创建时间：2011-08-15
 *     说明：打印表单配置
 * **********************************************************************************************************************************************/
using System;
using System.Collections.Generic;
using System.Text;
using System.Data;
using Epower.DevBase.BaseTools;
using System.Data.OracleClient;
using EpowerCom;
using EpowerGlobal;

namespace Epower.ITSM.SqlDAL.Print
{

    [Serializable]
    /// <summary>
    /// 打印表单规则配置
    /// </summary>  
    public class PRINTRULE
    {

        #region 属性
        private long IntID;
        /// <summary>
        /// 主键ID
        /// </summary>
        public long ID //--主键ID
        {
            get{
                return IntID;
            }
            set
            {
                IntID=value;
            }
        }

        private string  strPrintRuleName=string.Empty;
        /// <summary>
        /// 打印规则名称
        /// </summary>
        public string PrintRuleName 
        {
               get{
                return strPrintRuleName;
               }
            set
            {
                strPrintRuleName=value;
            }
        
        }


        private long lngAppId;
        /// <summary>
        /// 应用id
        /// </summary>
        public long AppId 
        {
            get{
                return lngAppId;
            }
            set
            {
                lngAppId=value;
            }
        }
	
	
        private string  strAppNames=string.Empty;
        /// <summary>
        /// 应用名称
        /// </summary>
        public string AppNames 
        {
               get{
                return strAppNames;
               }
            set
            {
                strAppNames=value;
            }        
        }
        

          private long lngFlowModelId;
        /// <summary>
        /// 流程模型id
        /// </summary>
        public long FlowModelId 
        {
            get{
                return lngFlowModelId;
            }
            set
            {
                lngFlowModelId=value;
            }
        }

        private string  strFlowModelName=string.Empty;
        /// <summary>
        /// 流程模型名称
        /// </summary>
        public string FlowModelName 
        {
               get{
                return strFlowModelName;
               }
            set
            {
                strFlowModelName=value;
            }        
        }

        
        private string  strModelContent=string.Empty;
        /// <summary>
        /// 打印模板内容
        /// </summary>
        public string ModelContent 
        {
               get{
                return strModelContent;
               }
            set
            {
                strModelContent=value;
            }        
        }


         private int intIsOpen;
        /// <summary>
        /// 是否启用
        /// </summary>
        public int IsOpen 
        {
            get{
                return intIsOpen;
            }
            set
            {
                intIsOpen=value;
            }
        }

         private string  strremark=string.Empty;
        /// <summary>
        /// 备注
        /// </summary>
        public string remark 
        {
               get{
                return strremark;
               }
            set
            {
                strremark=value;
            }        
        }
        
    private long lngcreateByID;
        /// <summary>
        /// 创建人id
        /// </summary>
        public long createByID 
        {
            get{
                return lngcreateByID;
            }
            set
            {
                lngcreateByID=value;
            }
        }

         private string  strcreateByName=string.Empty;
        /// <summary>
        /// 创建人姓名
        /// </summary>
        public string createByName 
        {
               get{
                return strcreateByName;
               }
            set
            {
                strcreateByName=value;
            }       
        }

        private long lngmodifyByID;
        /// <summary>
        /// 修改人id
        /// </summary>
        public long modifyByID 
        {
            get{
                return lngmodifyByID;
            }
            set
            {
                lngmodifyByID=value;
            }
        }

         private string  strmodifyByName=string.Empty;
        /// <summary>
        /// 修改人姓名
        /// </summary>
        public string modifyByName 
        {
               get{
                return strmodifyByName;
               }
            set
            {
                strmodifyByName=value;
            }
        }
        private Int32 nIsProcess = 0;
        /// <summary>
        /// 是否打印处理过程
        /// </summary>
        public Int32 IsProcess
        {
            get
            {
                return nIsProcess;
            }
            set
            {
                nIsProcess = value;
            }
        }
        #endregion 
        /// <summary>
        /// 构造函数
        /// </summary>
        public PRINTRULE()
        {
            //构造函数
        }

        /// <summary>
        /// 查询单条记录
        /// </summary>
        /// <param name="ID"></param>
        /// <returns></returns>
        public PRINTRULE select(long ID)
        {
            string strSQL = " select * from PrintRule where deleted=" + Convert.ToInt32(e_Deleted.eNormal) +" and  ID =" + ID.ToString();
            DataTable dt= CommonDP.ExcuteSqlTable(strSQL);
            PRINTRULE entity=null;
            if (dt.Rows.Count > 0)
            {
                entity = new PRINTRULE();
                entity.ID = long.Parse(dt.Rows[0]["ID"].ToString());
                entity.PrintRuleName = dt.Rows[0]["PrintRuleName"].ToString();
                entity.AppId = long.Parse(dt.Rows[0]["AppId"].ToString());
                entity.AppNames = dt.Rows[0]["AppNames"].ToString();
                entity.FlowModelId = long.Parse(dt.Rows[0]["FlowModelId"].ToString());
                entity.FlowModelName = dt.Rows[0]["FlowModelName"].ToString();
                entity.remark = dt.Rows[0]["remark"].ToString();
                entity.ModelContent = dt.Rows[0]["ModelContent"].ToString();
                entity.createByID = long.Parse(dt.Rows[0]["createByID"].ToString());
                entity.createByName = dt.Rows[0]["createByName"].ToString();
                entity.IsOpen = int.Parse(dt.Rows[0]["IsOpen"].ToString());
                entity.modifyByID = long.Parse(dt.Rows[0]["modifyByID"].ToString());
                entity.modifyByName = dt.Rows[0]["modifyByName"].ToString();
                entity.IsProcess = Int32.Parse(dt.Rows[0]["IsProcess"].ToString() == "" ? "0" : dt.Rows[0]["IsProcess"].ToString());
            }
            return entity;

        }

        /// <summary>
        /// 查询出一个数据集
        /// </summary>
        /// <returns>返回 DataTable 集合</returns>
        public DataTable select()
        {
            string strSQL = " select * from PrintRule where deleted=" + Convert.ToInt32(e_Deleted.eNormal) ;
            DataTable dt = CommonDP.ExcuteSqlTable(strSQL);
            return dt;
        }


        /// <summary>
        /// 存储过程翻页
        /// </summary>
        /// <param name="sWhere"></param>
        /// <param name="sOrder"></param>
        /// <param name="pagesize"></param>
        /// <param name="pageindex"></param>
        /// <param name="rowcount"></param>
        /// <returns></returns>
        public DataTable GetDataTable(string sWhere, string sOrder, int pagesize, int pageindex, ref int rowcount)
        {
            OracleConnection cn = ConfigTool.GetConnection();
            DataTable dt = OracleDbHelper.ExecuteDataTable(cn, "PrintRule", "*", sOrder, pagesize, pageindex, sWhere, ref rowcount);
            ConfigTool.CloseConnection(cn);
            return dt;
        }

        public void delete(long ID)
        {
            string strSQL = "update  PrintRule set deleted="+ Convert.ToInt32(e_Deleted.eDeleted)+"   where  ID=" + ID.ToString();
            CommonDP.ExcuteSql(strSQL);
        }


        /// <summary>
        /// 保存
        /// </summary>
        public PRINTRULE Save(PRINTRULE Entity)
        {

            string strSQL = "";
            if (Entity.ID == null || Entity.ID == 0)
            {
                Entity.ID = EPGlobal.GetNextID("PrintRuleID");
                strSQL= @" insert into PrintRule(
                                ID,
	                            PrintRuleName,--打印规则名称
	                            AppId,--应用id
	                            AppNames,--应用名称
	                            FlowModelId,--流程模型id
	                            FlowModelName,--流程模型名称		                            
	                            IsOpen, --是否启用	
	                            remark,--备注
	                            createByID,--创建人id
	                            createByName,--创建人姓名
	                            modifyByID,--修改人id
	                            modifyByName,
                                IsProcess,
                                deleted
                                )
                values(
                        " + Entity.ID+","+
                         StringTool.SqlQ(Entity.PrintRuleName)+","+
                         Entity.AppId+","+
                         StringTool.SqlQ(Entity.AppNames)+","+
                         Entity.FlowModelId+","+
                         StringTool.SqlQ(Entity.FlowModelName)+","+                         
                         Entity.IsOpen+","+
                         StringTool.SqlQ(Entity.remark)+","+
                         Entity.createByID+","+
                         StringTool.SqlQ(Entity.createByName)+","+
                         Entity.modifyByID+","+
                         StringTool.SqlQ(Entity.modifyByName) + "," +
                         Entity.IsProcess + 
                        ",0)";
            }
            else
            {

                strSQL = @" UPDATE  PrintRule set " +
                                "PrintRuleName=" + StringTool.SqlQ(Entity.PrintRuleName) + "," +
                                "AppId=" + Entity.AppId + "," +
                                "AppNames=" + StringTool.SqlQ(Entity.AppNames) + "," +
                                "FlowModelId=" + Entity.FlowModelId + "," +
                                "FlowModelName=" + StringTool.SqlQ(Entity.FlowModelName) + "," +                                
                                "IsOpen=" + Entity.IsOpen + "," +
                                "remark=" + StringTool.SqlQ(Entity.remark) + "," +
                                "createByID=" + Entity.createByID + "," +
                                "createByName=" + StringTool.SqlQ(Entity.createByName) + "," +
                                "modifyByID=" + Entity.modifyByID + "," +
                                "modifyByName=" + StringTool.SqlQ(Entity.modifyByName) + "," +
                                " IsProcess = " + Entity.IsProcess.ToString() +
                                " WHERE  ID=" + Entity.ID;
                        
                         
            }

            OracleConnection cn = ConfigTool.GetConnection();
            if (cn.State != ConnectionState.Open)
                cn.Open();

            OracleTransaction trans = cn.BeginTransaction();
            
            try
            {   
                OracleDbHelper.ExecuteNonQuery(trans, CommandType.Text, strSQL);

                #region 保存打印模板内容信息，避免字符串超长出错 余向前 2013-03-18
                strSQL = "update PrintRule set ModelContent=:a where ID = " + Entity.ID.ToString();
                OracleCommand cmd = new OracleCommand(strSQL, trans.Connection, trans);
                cmd.Parameters.Add("a", OracleType.Clob).Value = Entity.ModelContent;
                cmd.ExecuteNonQuery();
                #endregion

                trans.Commit();
            }
            catch
            {
                trans.Rollback();
            }
            finally
            {
                ConfigTool.CloseConnection(cn);
            }
            return Entity;

        }


        /// <summary>
        /// 获得打印的输出
        /// </summary>
        /// <param name="lngFlowModelId"></param>
        /// <param name="fv"></param>
        public static string[]  getPrintContent(long lngFlowModelId, DataTable dtRows)
        {
            long lngAppId = 0;//应用id
            string flowname = ""; //流程名称
            string ModelContent = string.Empty;//打印模板内容
            string isProcess=string.Empty;//处理过程
            string [] arrContent=new string[2];
            long lngOFlowModeId = MailAndMessageRuleDP.getEmailModel(lngFlowModelId, ref lngAppId, ref flowname);//获得流程模型的模型id

            //流程模型存在的情况判断
            if (lngAppId != 0 && lngOFlowModeId != 0)
            {
                string strSQL = @" select * from printrule 
                                    where AppId="+lngAppId.ToString()+@"
                                      and FlowModelId="+lngOFlowModeId.ToString()+@"
                                      and deleted="+Convert.ToInt16(e_Deleted.eNormal)+@"
                                      and IsOpen=1";

                DataTable dt = CommonDP.ExcuteSqlTable(strSQL);

                
                if (dt.Rows.Count > 0)
                {
                    ModelContent = dt.Rows[0]["ModelContent"].ToString();//内容
                    isProcess=dt.Rows[0]["IsProcess"].ToString();//处理过程
                }
                //替换后的内容
                ModelContent = getContent(lngAppId, ModelContent, dtRows);

            }
            arrContent[0] = ModelContent;//内容
            arrContent[1] = isProcess;//处理过程
            return arrContent;

        }

        #region 获取各表单不同的信息项内容
        /// <summary>
        /// 获取各表单不同的信息项内容
        /// </summary>
        /// <param name="appId"></param>
        /// <param name="content"></param>
        /// <param name="fv"></param>
        /// <returns></returns>
        public static string getContent(long appId, string content, DataTable dt)
        {
            switch (appId)
            {
                   
                case 1028://发布管理
                    content = content.Replace(",#?版本名称?#", dt.Rows[0]["VERSIONNAME"].ToString()).Replace(",#?版本号?#", dt.Rows[0]["VERSIONCODE"].ToString()).Replace(",#?发布范围?#", dt.Rows[0]["RELEASESCOPENAME"].ToString()).Replace(",#?联系人?#", dt.Rows[0]["RELEASEPERSONNAME"].ToString()).Replace(",#?联系电话?#", dt.Rows[0]["RELEASEPHONE"].ToString());
                    content = content.Replace(",#?版本性质?#", dt.Rows[0]["VERSIONKINDNAME"].ToString()).Replace(",#?版本类型?#", dt.Rows[0]["VERSIONTYPENAME"].ToString()).Replace(",#?版本发布内容简介?#", dt.Rows[0]["RELEASECONTENT"].ToString());
                    break;
                case 1062://需求管理
                    content = content.Replace("#?客户名称?#", dt.Rows[0]["CustUserName"].ToString());
                    content = content.Replace("#?客户地址?#", dt.Rows[0]["CustAddress"].ToString());
                    content = content.Replace("#?联系人?#", dt.Rows[0]["CustContact"].ToString());
                    content = content.Replace("#?联系电话?#", dt.Rows[0]["CustTel"].ToString());
                    content = content.Replace("#?客户部门?#", dt.Rows[0]["CustDeptName"].ToString());
                    content = content.Replace("#?电子邮件?#", dt.Rows[0]["CustEmail"].ToString());
                    content = content.Replace("#?服务单位?#", dt.Rows[0]["CustMastName"].ToString());
                    content = content.Replace("#?职位?#", dt.Rows[0]["CustJob"].ToString());

                    content = content.Replace("#?资产名称?#", dt.Rows[0]["EquipmentName"].ToString());
                    content = content.Replace("#?登单人?#", dt.Rows[0]["RegUserName"].ToString());
                    content = content.Replace("#?登记时间?#", dt.Rows[0]["RegTime"].ToString() == null ? "" : dt.Rows[0]["RegTime"].ToString());

                    content = content.Replace("#?需求单号?#", dt.Rows[0]["DemandNo"].ToString());
                    content = content.Replace("#?需求类别?#", dt.Rows[0]["DemandTypeName"].ToString());
                    content = content.Replace("#?需求状态?#", dt.Rows[0]["DemandStatus"].ToString());
                    content = content.Replace("#?需求主题?#", dt.Rows[0]["DemandSubject"].ToString());
                    content = content.Replace("#?详细描述?#", dt.Rows[0]["DemandContent"].ToString());                  
                    break;
                case 201://自定义表单流程
                    break;
                case 400://知识管理                  
                    content = content.Replace("#?主题?#", dt.Rows[0]["Title"].ToString());
                    content = content.Replace("#?关键字?#", dt.Rows[0]["Pkey"].ToString());
                    content = content.Replace("#?摘要?#", dt.Rows[0]["tags"].ToString());
                    content = content.Replace("#?知识类别?#", dt.Rows[0]["Type"].ToString());
                    content = content.Replace("#?资产名称?#", dt.Rows[0]["EquName"].ToString());
                    content = content.Replace("#?知识内容?#", dt.Rows[0]["Content"].ToString());
                    content = content.Replace("#?同意入库?#", dt.Rows[0]["IsInKB"].ToString() == "1" ? "是" : "否");
                    break;
                case 199://通用流程       
                    break;
                case 210://问题管理                    
                    content = content.Replace("#?问题单号?#", dt.Rows[0]["BuildCode"].ToString() + dt.Rows[0]["ProblemNo"].ToString());
                    content = content.Replace("#?登记人?#", dt.Rows[0]["RegUserName"].ToString());
                    content = content.Replace("#?登记人部门?#", dt.Rows[0]["RegDeptName"].ToString());
                    content = content.Replace("#?登记时间?#", dt.Rows[0]["RegTime"].ToString());
                    content = content.Replace("#?问题类别?#", dt.Rows[0]["Problem_TypeName"].ToString());
                    content = content.Replace("#?问题级别?#", dt.Rows[0]["Problem_LevelName"].ToString());
                    content = content.Replace("#?影响度?#", dt.Rows[0]["EffectName"].ToString());
                    content = content.Replace("#?紧急度?#", dt.Rows[0]["InstancyName"].ToString());
                    content = content.Replace("#?资产名称?#", dt.Rows[0]["EquName"].ToString());
                    content = content.Replace("#?问题状态?#", dt.Rows[0]["StateName"].ToString());
                    content = content.Replace("#?摘要?#", dt.Rows[0]["Problem_Title"].ToString());
                    content = content.Replace("#?问题描述?#", dt.Rows[0]["Problem_Subject"].ToString());                    
                    content = content.Replace("#?解决方案?#", dt.Rows[0]["Remark"].ToString());
                    break;
                case 410://资产巡检 
                    content = content.Replace("#?标题?#", dt.Rows[0]["Title"].ToString());
                    content = content.Replace("#?登记人?#", dt.Rows[0]["RegUserName"].ToString());
                    content = content.Replace("#?登记部门?#", dt.Rows[0]["RegDeptName"].ToString());
                    content = content.Replace("#?登记时间?#", dt.Rows[0]["RegTime"].ToString());
                    content = content.Replace("#?备注?#", dt.Rows[0]["Remark"].ToString());

                    #region 替换巡检信息
                    if (content.Contains("#?巡检信息?#"))
                    {
                        DataTable dtItem = Equ_PatrolItemDataDP.GetPatrolItem(decimal.Parse(dt.Rows[0]["id"].ToString()));
                        string sTable = "";
                        if (dtItem != null && dtItem.Rows.Count > 0)
                        {
                            sTable += "<table width='100%'>"
                                    + "<tr>"
                                    + "<td>资产名称"
                                    + "</td>"
                                    + "<td>巡检项"
                                    + "</td>"
                                    + "<td>正常"
                                    + "</td>"
                                    + "<td>巡检时间"
                                    + "</td>"
                                    + "<td>巡检人"
                                    + "</td>"
                                    + "<td>备注"
                                    + "</td>"
                                    + "</tr>";

                        }

                        foreach (DataRow dr in dtItem.Rows)
                        {
                            sTable += "<tr>"
                                    + "<td>" + dr["EquName"].ToString()
                                    + "</td>"
                                    + "<td>" + dr["ItemName"].ToString()
                                    + "</td>"
                                    + "<td>" + (dr["ItemData"].ToString() == "0" ? "正常" : "不正常")
                                    + "</td>"
                                    + "<td>" + dr["PatrolTime"].ToString()
                                    + "</td>"
                                    + "<td>" + dr["PatrolName"].ToString()
                                    + "</td>"
                                    + "<td>" + dr["Remark"].ToString()
                                    + "</td>"
                                    + "</tr>";

                        }
                        if (dtItem != null && dtItem.Rows.Count > 0)
                        {
                            sTable += "</table>";
                        }

                        content = content.Replace("#?巡检信息?#", sTable);
                    }
                    #endregion
                    break;
                case 420://变更管理                   
                    content = content.Replace("#?客户名称?#", dt.Rows[0]["CustName"].ToString());
                    content = content.Replace("#?客户地址?#", dt.Rows[0]["CustAddress"].ToString());
                    content = content.Replace("#?联系人?#", dt.Rows[0]["Contact"].ToString());
                    content = content.Replace("#?联系电话?#", dt.Rows[0]["CTel"].ToString());
                    content = content.Replace("#?客户部门?#", dt.Rows[0]["CustDeptName"].ToString());
                    content = content.Replace("#?电子邮件?#", dt.Rows[0]["CustEmail"].ToString());
                    content = content.Replace("#?服务单位?#", dt.Rows[0]["MastCustName"].ToString());
                    content = content.Replace("#?职位?#", dt.Rows[0]["job"].ToString());

                    #region 替换资产信息
                    if (content.Contains("#?资产信息?#"))
                    {
                        DataTable dtItem = ChangeDealDP.GetCLFareItem(long.Parse(dt.Rows[0]["id"].ToString()));
                        string sTable = "";
                        if (dtItem != null && dtItem.Rows.Count > 0)
                        {
                            sTable += "<table width='100%'>"
                                    + "<tr>"
                                    + "<td>资产名称"
                                    + "</td>"
                                    + "<td>维护部门"
                                    + "</td>"
                                    + "<td>资产编号"
                                    + "</td>"
                                    + "<td>变更内容"
                                    + "</td>"
                                    + "</tr>";
                                
                        }

                        foreach (DataRow dr in dtItem.Rows)
                        {
                            sTable +="<tr>"
                                    + "<td>" + dr["EQUNAME"].ToString()
                                    + "</td>"
                                    + "<td>" + dr["DEPT"].ToString()
                                    + "</td>"
                                    + "<td>" + dr["EQUCODE"].ToString()
                                    + "</td>"
                                    + "<td>" + dr["CHANGECONTENT"].ToString()
                                    + "</td>"
                                    + "</tr>";

                        }
                        if (dtItem != null && dtItem.Rows.Count > 0)
                        {
                            sTable += "</table>";
                        }

                        content = content.Replace("#?资产信息?#", sTable);
                    }
                    #endregion

                    content = content.Replace("#?变更单号?#", dt.Rows[0]["ChangeNo"].ToString());
                    content = content.Replace("#?登记时间?#", dt.Rows[0]["RegTime"].ToString() == null ? "" : dt.Rows[0]["RegTime"].ToString());
                    content = content.Replace("#?变更类别?#", dt.Rows[0]["ChangeTypeName"].ToString());
                    content = content.Replace("#?变更时间?#", dt.Rows[0]["ChangeTime"].ToString() == null ? "" : dt.Rows[0]["ChangeTime"].ToString());
                    content = content.Replace("#?摘要?#", dt.Rows[0]["Subject"].ToString());
                    content = content.Replace("#?请求内容?#", dt.Rows[0]["Content"].ToString());
                    content = content.Replace("#?影响度?#", dt.Rows[0]["EffectName"].ToString());
                    content = content.Replace("#?紧急度?#", dt.Rows[0]["InstancyName"].ToString());
                    content = content.Replace("#?变更级别?#", dt.Rows[0]["LevelName"].ToString());
                    content = content.Replace("#?变更状态?#", dt.Rows[0]["DealStatus"].ToString());
                    content = content.Replace("#?变更分析?#", dt.Rows[0]["ChangeAnalyses"].ToString());
                    content = content.Replace("#?分析结果?#", dt.Rows[0]["ChangeAnalysesResult"].ToString());
                    break;
                case 1026://事件管理                    
                    content = content.Replace("#?客户名称?#", dt.Rows[0]["CustName"].ToString());
                    content = content.Replace("#?客户地址?#", dt.Rows[0]["CustAddress"].ToString());
                    content = content.Replace("#?联系人?#", dt.Rows[0]["Contact"].ToString());
                    content = content.Replace("#?联系电话?#", dt.Rows[0]["CTel"].ToString());
                    content = content.Replace("#?客户部门?#", dt.Rows[0]["CustDeptName"].ToString());
                    content = content.Replace("#?电子邮件?#", dt.Rows[0]["Email"].ToString());
                    content = content.Replace("#?服务单位?#", dt.Rows[0]["MastCust"].ToString());
                    content = content.Replace("#?职位?#", dt.Rows[0]["job"].ToString());
                    content = content.Replace("#?资产名称?#", dt.Rows[0]["EquipmentName"].ToString());
                    content = content.Replace("#?登单人?#", dt.Rows[0]["RegUserName"].ToString());
                    content = content.Replace("#?登记时间?#", dt.Rows[0]["RegSysDate"].ToString());
                    content = content.Replace("#?事件单号?#", dt.Rows[0]["BuildCode"].ToString() + dt.Rows[0]["ServiceNo"].ToString());
                    content = content.Replace("#?事件类别?#", dt.Rows[0]["ServiceType"].ToString());
                    content = content.Replace("#?发生时间?#", dt.Rows[0]["CustTime"].ToString() == null ? "" : dt.Rows[0]["CustTime"].ToString());
                    content = content.Replace("#?报告时间?#", dt.Rows[0]["ReportingTime"].ToString() == null ? "" : dt.Rows[0]["ReportingTime"].ToString());
                    content = content.Replace("#?紧急度?#", dt.Rows[0]["InstancyName"].ToString());
                    content = content.Replace("#?影响度?#", dt.Rows[0]["EffectName"].ToString());
                    content = content.Replace("#?摘要?#", dt.Rows[0]["Subject"].ToString());
                    content = content.Replace("#?需求描述?#", dt.Rows[0]["Content"].ToString());
                    content = content.Replace("#?服务级别?#", dt.Rows[0]["ServiceLevel"].ToString());
                    content = content.Replace("#?关闭理由?#", dt.Rows[0]["CloseReasonName"].ToString());
                    content = content.Replace("#?事件来源?#", dt.Rows[0]["ReSouseName"].ToString());
                    content = content.Replace("#?事件状态?#", dt.Rows[0]["DealStatus"].ToString());
                    content = content.Replace("#?完成时间?#", dt.Rows[0]["finishedtime"].ToString() == null ? "" : dt.Rows[0]["finishedtime"].ToString());
                    content = content.Replace("#?派出时间?#", dt.Rows[0]["outtime"].ToString() == null ? "" : dt.Rows[0]["outtime"].ToString());
                    content = content.Replace("#?上门时间?#", dt.Rows[0]["servicetime"].ToString() == null ? "" : dt.Rows[0]["servicetime"].ToString());
                    content = content.Replace("#?工程师?#", dt.Rows[0]["sjwxr"].ToString());
                    content = content.Replace("#?措施及结果?#", dt.Rows[0]["dealcontent"].ToString());
                    break;
                default:
                    break;
            }

            return content;
        }
        #endregion
        

    }
}
