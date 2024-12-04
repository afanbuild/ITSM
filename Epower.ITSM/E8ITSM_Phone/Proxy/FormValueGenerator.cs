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
using EpowerCom;
using Epower.ITSM.SqlDAL;
using System.Xml;
using Epower.DevBase.Organization.SqlDAL;
using EpowerGlobal;

namespace E8ITSM_Phone.Proxy
{
    /// <summary>
    /// 生成传递给流程引擎的内容表单
    /// </summary>
    public class FormValueGenerator
    {
        /// <summary>
        /// 用户编号
        /// </summary>
        private long _lngUserId;
        /// <summary>
        /// 表单内容集
        /// </summary>
        private FieldValues fv;

        public FormValueGenerator(long lngUserId)
        {
            this._lngUserId = lngUserId;
            this.fv = new FieldValues();
        }

        /// <summary>
        /// 取客户基础资料
        /// </summary>
        /// <param name="lngUserId">用户编号</param>        
        public void AddCustomerInfo()
        {
            Br_ECustomerDP ec = new Br_ECustomerDP();
            UserEntity user = new UserEntity(_lngUserId);

            ec = ec.GetReCordedByUserID(_lngUserId.ToString());


            // fv.Add("CustID", "8531");
            fv.Add("CustID", ec.ID.ToString());

            // fv.Add("CustName", "利多");
            fv.Add("CustName", ec.ShortName);
            fv.Add("CustAddress", ec.Address);
            fv.Add("Contact", ec.LinkMan1);
            fv.Add("CTel", ec.Tel1);
            // fv.Add("CustDeptName", "利多部");
            fv.Add("CustDeptName", ec.CustDeptName);
            // fv.Add("Job", "--");
            fv.Add("Job", ec.Job);
            fv.Add("Email", ec.Email);
            fv.Add("MastCust", ec.MastCustName);
        }

        /// <summary>
        /// 取客户资产资料
        /// </summary>
        /// <param name="lngUserId">用户编号</param>        
        public void AddEquipmentInfo()
        {
            Equ_DeskDP ee = new Equ_DeskDP();
            ee = ee.GetEquByCustID(_lngUserId);

            fv.Add("EquipmentID", ee.ID.ToString());
            fv.Add("EquipmentName", ee.Name);
            fv.Add("EquipmentCatalogName", ee.ListName); //资产目录
            fv.Add("EquipmentCatalogID", ee.ListID.ToString());
        }

        public String AddOtherInfo(long lngServiceListId)
        {
            #region 必须字段

            long lngSMSId = EPGlobal.GetNextID("ZHService_ID");

            fv.Add("ServiceID", lngSMSId.ToString());    // smsid

            // 取登单人名字
            Br_ECustomerDP customerDP = new Br_ECustomerDP();
            customerDP = customerDP.GetReCordedByUserID(_lngUserId.ToString());
            string strRegUserName = customerDP.RegUserName;

            fv.Add("RegUserName", strRegUserName);    // 登单人名字

            EA_ServicesTemplateDP ee = new EA_ServicesTemplateDP();
            ee = ee.GetReCorded(lngServiceListId);

            String strSubject = String.Format("{0}--{1}", ee.TemplateName, strRegUserName);

            fv.Add("Subject", String.Empty);    // 标题或摘要
            fv.Add("RegUserID", _lngUserId.ToString());    // 登单人编号            
            #endregion 必须字段


            #region 服务级别相关字段
            fv.Add("ServiceLevelID", "0");
            fv.Add("ServiceLevel", String.Empty);
            fv.Add("ServiceLevelID", "0");
            fv.Add("ServiceLevel", String.Empty);
            fv.Add("ServiceLevelChange", "false");
            #endregion 服务级别相关字段


            fv.Add("ServiceTypeID", "0");    // 事件类别编号

            fv.Add("ServiceType", String.Empty);    // 事件类型
            fv.Add("ServiceKindID", "0");  // 事件性质编号
            fv.Add("ServiceKind", String.Empty);    // 事件性质名


            fv.Add("EffectID", "0");    // 影响度编号
            fv.Add("EffectName", String.Empty);    // 影响度名字

            fv.Add("InstancyID", "0");    // 紧急度编号
            fv.Add("InstancyName", String.Empty);    // 紧急度名字

            // this.
            // fv.Add("DealStatusID", "1002");   //事件状态ID, 默认为 0
            fv.Add("DealStatusID", "0");   //事件状态ID, 默认为 0
            // fv.Add("DealStatus", "事件派单");    //事件状态, 默认为空
            fv.Add("DealStatus", String.Empty);    //事件状态, 默认为空


            fv.Add("CustTime", String.Empty);    // 事件发生时间
            fv.Add("ReportingTime", String.Empty);    // 事件报告时间            

            #region 资产信息

            fv.Add("EquPositions", String.Empty);    // 起草单时, 默认为空
            fv.Add("EquCode", String.Empty);    // 起草单时, 默认为空
            fv.Add("EquSN", String.Empty);    // 起草单时, 默认为空
            fv.Add("EquModel", String.Empty);    // 起草单时, 默认为空
            fv.Add("EquBreed", String.Empty);    // 起草单时, 默认为空

            #endregion

            fv.Add("DealContent", String.Empty);    // 措施及结果
            fv.Add("Outtime", String.Empty);    // 派出时间
            fv.Add("ServiceTime", String.Empty);    // 上门时间
            fv.Add("FinishedTime", String.Empty);    // 处理完成时间
            fv.Add("SjwxrID", String.Empty);    // 执行人编号
            fv.Add("Sjwxr", String.Empty);    // 执行人名字
            //fv.Add("TotalHours", txtTotalHours.Text.Trim());
            fv.Add("TotalAmount", "0");    // 总计金额
            fv.Add("OrgID", "0");    // Session["UserOrgID"].ToString() ???
            //fv.Add("ChangeProlem", txtChangeProlem.Text.Trim());
            fv.Add("RegSysDate", DateTime.Now.ToString());    // ?
            fv.Add("RegSysUserID", _lngUserId.ToString());    // ?
            fv.Add("RegSysUser", strRegUserName);    // Session["PersonName"].ToString()??

            fv.Add("EmailNotify", "false");    // 是否发送邮件
            fv.Add("SMSNotify", "false");    // 是否短信通知

            ////如果登单人不同，取相应的部门和部门名称
            //if (Session["UserID"].ToString() == RegUser.UserID.ToString())
            //{

            long lngUserDpetId = UserDP.GetUserDeptID(_lngUserId);
            String strUserDeptName = DeptDP.GetDeptName(lngUserDpetId);

            fv.Add("RegDeptID", lngUserDpetId.ToString());    //  填单者部门编号
            fv.Add("RegDeptName", strUserDeptName);    // 填单者部门名字
            //}
            //else
            //{
            //    Epower.DevBase.Organization.SqlDAL.UserEntity pUserEntity = new UserEntity(RegUser.UserID);
            //    fv.Add("RegDeptID", pUserEntity.DeptID.ToString());
            //    fv.Add("RegDeptName", pUserEntity.FullDeptName);
            //}

            //取得服务单前缀，生成服务单号
            string sServiceNo = String.Empty;
            if (sServiceNo == string.Empty)
            {
                string sBuildCode = string.Empty;
                sServiceNo = RuleCodeDP.GetCodeBH2(10003, ref sBuildCode);
                fv.Add("serviceno", sServiceNo);    // 生成服务单号
                fv.Add("buildCode", sBuildCode);    // 服务单前缀
            }



            fv.Add("Flag", "false");  //区分标志

            fv.Add("CloseReasonID", "0");    //关闭理由ID
            fv.Add("CloseReasonName", String.Empty);    //关闭理由名称
            fv.Add("ReSouseID", "0");    //事件来源ID
            fv.Add("ReSouseName", String.Empty);    //事件来源名称

            fv.Add("pubRequestID", "0");   //2009-04-25 增加 公众请求ID


            // this.
            //事件模板相关
            fv.Add("IssTempID", ee.IssTempID.ToString());
            fv.Add("IsUseIssTempID", "1");
            fv.Add("CustAreaID", "0");
            fv.Add("CustArea", "");
            fv.Add("ApplicationTime", "");
            fv.Add("ExpectedTime", "");
            fv.Add("Reason", "");


            #region 结束事件单号

            fv.Add("ItemCount", String.Empty);    // 扩展项 数
            fv.Add("ItemXml", String.Empty);    // 扩展项 XML

            fv.Add("ExtensionDayList", String.Empty);  //扩展项

            #endregion

            return strSubject;
        }

        /// <summary>
        /// 取事件模版预设值
        /// </summary>
        /// <param name="lngServiceListId"></param>
        public void AddIssueTemplateInfo(long lngServiceListId)
        {
            // 在服务目录中取事件模版编号
            EA_ServicesTemplateDP ee = new EA_ServicesTemplateDP();
            DataTable dt = ee.GetDataTable(String.Format(@" and TEMPLATEID = {0} ",
                lngServiceListId), String.Empty);
            long lngTemplateId = long.Parse(dt.Rows[0]["ISSTEMPID"].ToString());

            // 取事件模版
            EA_ShortCutTemplateDP template = new EA_ShortCutTemplateDP()
                                             .GetReCorded(lngTemplateId);

            // 将事件模版中预设值放入 fv 集中
            XmlDocument xml_doc = new XmlDocument();
            xml_doc.LoadXml(template.TemplateXml);

            XmlNodeList node_list = xml_doc.ChildNodes[0].ChildNodes;
            for (int i = 0; i < node_list.Count; i++)
            {
                XmlNode node = node_list[i];

                for (int j = 0; j < fv.Count; j++)
                {
                    Boolean isEquals = fv[j].ID.ToLower().Equals(node.Attributes[0].Value.ToLower());
                    if (isEquals)
                    {
                        fv[j].Value = node.Attributes[1].Value;
                        break;
                    }
                }

                //fv.Add(new FieldValue(node.Attributes[0].Value, node.Attributes[1].Value));
            }
        }

        /// <summary>
        /// 添加项
        /// </summary>
        /// <param name="strKey"></param>
        /// <param name="strVal"></param>
        public void AddFieldValue(string strKey, string strVal)
        {
            fv.Add(strKey, strVal);
        }
        /// <summary>
        /// 返回表单内容 XML 串
        /// </summary>
        /// <returns></returns>
        public override string ToString()
        {
            return fv.GetXmlObject().InnerXml;
        }
    }
}
