using System;
using System.Collections;
using System.Configuration;
using System.Data;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using Epower.ITSM.SqlDAL;
using System.Text;
using Epower.ITSM.SqlDAL.XmlHttp;
using EpowerCom;
using EpowerGlobal;

namespace Epower.ITSM.Web.AppForms
{
    public partial class frmBr_CustomXmlHttp : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (Request.QueryString["formurl"] != null)
            {
                Session["FromUrl"] = Request.QueryString["formurl"].ToString();
            }
            else if (Request.QueryString["id"] != null)
            {
                Br_ECustomerDP ee = new Br_ECustomerDP();
                if (Request.QueryString["id"].ToString() != string.Empty)
                {
                    ee = ee.GetReCorded(long.Parse(Request.QueryString["id"].ToString()));

                    StringBuilder sb = new StringBuilder();
                    if (ee.ID != 0)
                    {
                        sb.Append("<table class='listContent' width='380px' >");
                        string ShortName = ee.ShortName == "" ? "--" : ee.ShortName;
                        string FullName = ee.FullName == "" ? "--" : ee.FullName;
                        sb.Append("<tr><td class='listTitleNew_s' style='text-align:left;' width='17%'>用户名称</td><td class='listNew_s' style='text-align:left;' width='33%'>" + ShortName
                                    + "</td><td class='listTitleNew_s' style='text-align:left;' width='17%'>英文名称</td><td class='listNew_s' style='text-align:left;' width='33%'>" + FullName + "</td></tr>");
                        string CustomerTypeName=ee.CustomerTypeName == "" ? "--" : ee.CustomerTypeName;
                        string Address = ee.Address == "" ? "--" : ee.Address;
                        sb.Append("<tr><td class='listTitleNew_s' style='text-align:left;'>客户类型</td><td class='listNew_s' style='text-align:left;'>" + CustomerTypeName
                                    + "</td><td class='listTitleNew_s' style='text-align:left;'>客户地址</td><td class='listNew_s' style='text-align:left;'>" + Address + "</td></tr>");
                        string LinkMan1 = ee.LinkMan1 == "" ? "--" : ee.LinkMan1;
                        string CustomCode = ee.CustomCode == "" ? "--" : ee.CustomCode;
                        sb.Append("<tr><td class='listTitleNew_s' style='text-align:left;'>联系人</td><td class='listNew_s' style='text-align:left;'>" + LinkMan1
                                    + "</td><td class='listTitleNew_s' style='text-align:left;'>客户编号</td><td class='listNew_s' style='text-align:left;'>" + CustomCode + "</td></tr>");
                        string Tel1 =ee.Tel1 == "" ? "--" : ee.Tel1;
                        string Email = ee.Email == "" ? "--" : ee.Email;
                        sb.Append("<tr><td class='listTitleNew_s' style='text-align:left;'>联系电话</td><td class='listNew_s' style='text-align:left;'>" + Tel1
                                    + "</td><td class='listTitleNew_s' style='text-align:left;'>Email</td><td class='listNew_s' style='text-align:left;'>" + Email + "</td></tr>");

                        string CustDeptName=ee.CustDeptName == "" ? "--" : ee.CustDeptName;
                        string Rights = ee.Rights == "" ? "--" : ee.Rights;
                        sb.Append("<tr><td class='listTitleNew_s' style='text-align:left;'>用户部门</td><td class='listNew_s' style='text-align:left;'>" + CustDeptName
                                    + "</td><td class='listTitleNew_s' style='text-align:left;'>用户权限</td><td class='listNew_s' style='text-align:left;'>" + Rights + "</td></tr>");
                        string RegUserName = ee.RegUserName == "" ? "--" : ee.RegUserName;
                        string Remark = ee.Remark == "" ? "--" : ee.Remark;
                        sb.Append("<tr><td class='listTitleNew_s' style='text-align:left;'>登记人</td><td class='listNew_s' style='text-align:left;'>" + RegUserName
                                    + "</td><td class='listTitleNew_s' style='text-align:left;'>备注</td><td class='listNew_s' style='text-align:left;'>" + Remark + "</td></tr>");
                       
                        sb.Append("</table>");  
                    }
                    Response.Clear();
                    Response.Write(sb.ToString());
                    Response.Flush();
                    Response.End();
                }
            }
            else if (Request.QueryString["Issue"] != null)
            {
                EA_ShortCutTemplateDP Template = new EA_ShortCutTemplateDP();
                StringBuilder sb = new StringBuilder();
                if (Request.QueryString["Issue"].ToString() != string.Empty)
                {

                    DataTable dt = Template.getMytempLatiesXmlHttp(long.Parse(Request.QueryString["Issue"].ToString()));
                    if (dt.Rows.Count > 0)
                    {
                        sb.Append("<table class='listContent' width='380px' >");
                        if (dt.Rows[0]["Owner"].ToString().Trim() != "" || dt.Rows[0]["TemplateName"].ToString().Trim() != "")
                        {
                            string type = dt.Rows[0]["Owner"].ToString().Trim() == "0" ? "公共" : "个人";
                            sb.Append("<tr><td class='listTitleNew_s' style='text-align:left;' width='17%'>模版性质</td><td class='listNew_s' style='text-align:left;' width='33%'>" + type
                                    + "</td><td class='listTitleNew_s' style='text-align:left;' width='17%'>模版名称</td><td class='listNew_s' style='text-align:left;' width='33%'>" + dt.Rows[0]["TemplateName"].ToString().Trim() + "</td></tr>");
                        }
                        sb.Append("</table>");
                    }
                    Response.Clear();
                    Response.Write(sb.ToString());
                    Response.Flush();
                    Response.End();
                }
            }
            else if (Request.QueryString["ZHServiceDP"] != null)
            {
                StringBuilder sb = new StringBuilder();
                if (Request.QueryString["ZHServiceDP"].ToString() != string.Empty)
                {
                 
                    DataTable dt = ZHServiceDP.getIssues(long.Parse(Request.QueryString["ZHServiceDP"].ToString()));
                    if (dt.Rows.Count > 0)
                    {
                        //获取messageid
                        long lngMessageID = FlowDP.GetMessageId(long.Parse(dt.Rows[0]["flowid"].ToString()), (long)Session["UserID"]);
                        //取得权限控制
                        objFlow oFlow =  new objFlow((long)Session["UserID"], long.Parse(dt.Rows[0]["flowModelid"].ToString()), lngMessageID);
                        setFieldCollection setFields = oFlow.setFields;

                        string strCustName = string.Empty;
                        string strCustAddress = string.Empty;
                        string strcontact = string.Empty;
                        string strctel = string.Empty;
                        string strCustCode = string.Empty;
                        string strEmail = string.Empty;
                        string strMastCust = string.Empty;
                        string strjob = string.Empty;
                        if (setFields.GetsetField("custinfo", e_CondSetType.fmcstField)==null || setFields.GetsetField("custinfo", e_CondSetType.fmcstField).Visibled == false)
                        {
                            strCustName = "--";
                            strCustAddress = "--";
                            strcontact = "--";
                            strctel = "--";
                            strCustCode = "--";
                            strEmail = "--";
                            strMastCust = "--";
                            strjob = "--";
                        }
                        else
                        {
                            strCustName = dt.Rows[0]["CustName"].ToString().Trim() == "" ? "--" : dt.Rows[0]["CustName"].ToString().Trim();
                            strCustAddress = dt.Rows[0]["CustAddress"].ToString().Trim() == "" ? "--" : dt.Rows[0]["CustAddress"].ToString().Trim();
                            strcontact = dt.Rows[0]["contact"].ToString().Trim() == "" ? "--" : dt.Rows[0]["contact"].ToString().Trim();
                            strctel = dt.Rows[0]["ctel"].ToString().Trim() == "" ? "--" : dt.Rows[0]["ctel"].ToString().Trim();
                            strCustCode = dt.Rows[0]["CustDeptName"].ToString().Trim() == "" ? "--" : dt.Rows[0]["CustDeptName"].ToString().Trim();
                            strEmail = dt.Rows[0]["Email"].ToString().Trim() == "" ? "--" : dt.Rows[0]["Email"].ToString().Trim();
                            strMastCust = dt.Rows[0]["MastCust"].ToString().Trim() == "" ? "--" : dt.Rows[0]["MastCust"].ToString().Trim();
                            strjob = dt.Rows[0]["job"].ToString().Trim() == "" ? "--" : dt.Rows[0]["job"].ToString().Trim();
                        }

                        string EquipmentName = string.Empty;
                        string EquCode = string.Empty;
                        string EquSN = string.Empty;
                        string EquModel = string.Empty;
                        string EquBreed = string.Empty;
                        if (setFields.GetsetField("equinfo", e_CondSetType.fmcstField) == null || setFields.GetsetField("equinfo", e_CondSetType.fmcstField).Visibled == false)
                        {
                            EquipmentName = "--";
                            EquCode = "--";
                            EquSN = "--";
                            EquModel = "--";
                            EquBreed = "--";
                        }
                        else
                        {
                            EquipmentName = dt.Rows[0]["EquipmentName"].ToString().Trim() == "" ? "--" : dt.Rows[0]["EquipmentName"].ToString().Trim();
                            EquCode = dt.Rows[0]["EquCode"].ToString().Trim() == "" ? "--" : dt.Rows[0]["EquipmentCatalogName"].ToString().Trim();
                            EquSN = dt.Rows[0]["EquSN"].ToString().Trim() == "" ? "--" : dt.Rows[0]["EquSN"].ToString().Trim();
                            EquModel = dt.Rows[0]["EquModel"].ToString().Trim() == "" ? "--" : dt.Rows[0]["EquModel"].ToString().Trim();
                            EquBreed = dt.Rows[0]["EquBreed"].ToString().Trim() == "" ? "--" : dt.Rows[0]["EquBreed"].ToString().Trim();
                        }
                        
                        sb.Append("<table class='listContent' width='420px' >");
                     
                            sb.Append("<tr><td class='listTitleNew_s' style='text-align:left;' width='17%'>用名名称</td><td class='listNew_s' style='text-align:left;' width='33%'>" + strCustName
                                    + "</td><td class='listTitleNew_s' style='text-align:left;' width='17%'>用户地址</td><td class='listNew_s' style='text-align:left;' width='33%'>" + strCustAddress + "</td></tr>");                       

                            sb.Append("<tr><td class='listTitleNew_s' style='text-align:left;' width='17%'>联系人</td><td class='listNew_s' style='text-align:left;' width='33%'>" + strcontact
                                    + "</td><td class='listTitleNew_s' style='text-align:left;' width='17%'>联系电话</td><td class='listNew_s' style='text-align:left;' width='33%'>" + strctel + "</td></tr>");

                            sb.Append("<tr><td class='listTitleNew_s' style='text-align:left;' width='17%'>客户部门</td><td class='listNew_s' style='text-align:left;' width='33%'>" + strCustCode
                                    + "</td><td class='listTitleNew_s' style='text-align:left;' width='17%'>电子邮件</td><td class='listNew_s' style='text-align:left;' width='33%'>" + strEmail + "</td></tr>");                    

                            sb.Append("<tr><td class='listTitleNew_s' style='text-align:left;' width='17%'>服务单位</td><td class='listNew_s' style='text-align:left;' width='33%'>" + strMastCust
                                    + "</td><td class='listTitleNew_s' style='text-align:left;' width='17%'>职位</td><td class='listNew_s' style='text-align:left;' width='33%'>" + strjob + "</td></tr>");

                            //sb.Append("<tr><td class='listTitleNew_s' style='text-align:left;' width='17%'>资产目录</td><td class='listNew_s' style='text-align:left;' width='33%'>" + EquCode
                            sb.Append("<tr><td class='listTitleNew_s' style='text-align:left;' width='17%'>资产名称</td><td class='listNew_s' style='text-align:left;' width='33%' colspan='3'>" + EquipmentName + "</td></tr>");      
                     

                        DataTable dt2 = ZHServiceDP.GetCurrDealName(long.Parse(dt.Rows[0]["FlowID"].ToString()));
                        string sCurrNode = string.Empty;
                        string sCurrPerson = string.Empty;
                        if (dt2.Rows.Count > 0) 
                        {
                            for (int j = 0; j < dt2.Rows.Count; j++)
                            {
                                if (j == dt2.Rows.Count - 1)
                                {
                                    sCurrNode = dt2.Rows[j]["NodeName"].ToString();
                                    sCurrPerson = dt2.Rows[j]["Name"].ToString();
                                }
                                else
                                {
                                    sCurrNode = dt2.Rows[j]["NodeName"].ToString() + ",";
                                    sCurrPerson = dt2.Rows[j]["Name"].ToString() + ",";
                                }
                            }
                        }
                      sCurrPerson= sCurrPerson == "" ? "--" : sCurrPerson;
                       sCurrNode= sCurrNode == "" ? "--" : sCurrNode;
                        sb.Append("<tr><td class='listTitleNew_s' style='text-align:left;' width='17%'>当前处理人</td><td class='listNew_s' style='text-align:left; color:Red;' width='33%'>" + sCurrPerson 
                                  + "</td><td class='listTitleNew_s' style='text-align:left;' width='17%'>当前环节</td><td class='listNew_s' style='text-align:left; color:Red;' width='33%'>" + sCurrNode  + "</td></tr>");
                        sb.Append("</table>");
                    }
                    Response.Clear();
                    Response.Write(sb.ToString());
                    Response.Flush();
                    Response.End();
                }
            }
            else if (Request.QueryString["V_BYTS"] != null)
            {
                 StringBuilder sb = new StringBuilder();
                 if (Request.QueryString["V_BYTS"].ToString() != string.Empty)
                {

                    DataTable dt = XmlHttpTable.GETdatetable("V_BYTS", " and By_id="+Request.QueryString["V_BYTS"].ToString());
                    if (dt.Rows.Count > 0)
                    {
                        long lngMessageID = FlowDP.GetMessageId(long.Parse(dt.Rows[0]["flowid"].ToString()), (long)Session["UserID"]);
                        //取得权限控制
                        objFlow oFlow = new objFlow((long)Session["UserID"], long.Parse(dt.Rows[0]["flowModelid"].ToString()), lngMessageID);
                        setFieldCollection setFields = oFlow.setFields;

                        sb.Append("<table class='listContent' width='380px' >");
                   
                            //被投诉人
                            string BY_ProjectName = dt.Rows[0]["BY_ProjectName"].ToString().Trim() == "" ? "--" : dt.Rows[0]["BY_ProjectName"].ToString().Trim();
                           
                            //投诉人
                            string BY_PersonName = dt.Rows[0]["BY_PersonName"].ToString().Trim() == "" ? "--" : dt.Rows[0]["BY_PersonName"].ToString().Trim();
                            if (setFields.GetsetField("by_personname", e_CondSetType.fmcstField) == null || setFields.GetsetField("by_personname", e_CondSetType.fmcstField).Visibled == false)
                            {
                                BY_PersonName = "--";
                            }

                            sb.Append("<tr><td class='listTitleNew_s' style='text-align:left;' width='17%'>被投诉人</td><td class='listNew_s' style='text-align:left;' width='33%'>" + BY_ProjectName
                                + "</td><td class='listTitleNew_s' style='text-align:left;' width='17%'>投诉人</td><td class='listNew_s' style='text-align:left;' width='33%'>" + BY_PersonName + "</td></tr>");
                                    
                   
                        
                            //投诉类型
                            string BY_TypeName = dt.Rows[0]["BY_TypeName"].ToString().Trim() == "" ? "--" : dt.Rows[0]["BY_TypeName"].ToString().Trim();
                            if (setFields.GetsetField("by_type", e_CondSetType.fmcstField) == null || setFields.GetsetField("by_type", e_CondSetType.fmcstField).Visibled == false)
                            {
                                BY_TypeName = "--";
                            }
                            //投诉性质
                            string BY_KindName = dt.Rows[0]["BY_KindName"].ToString().Trim() == "" ? "--" : dt.Rows[0]["BY_KindName"].ToString().Trim();
                            if (setFields.GetsetField("by_kind", e_CondSetType.fmcstField) == null || setFields.GetsetField("by_kind", e_CondSetType.fmcstField).Visibled == false)
                            {
                                BY_KindName = "--";
                            }

                            sb.Append("<tr><td class='listTitleNew_s' style='text-align:left;' width='17%'>投诉类型</td><td class='listNew_s' style='text-align:left;' width='33%'>" + BY_TypeName
                                    + "</td><td class='listTitleNew_s' style='text-align:left;' width='17%'>投诉性质</td><td class='listNew_s' style='text-align:left;' width='33%'>" + BY_KindName + "</td></tr>");
                    
                     
                            //客户名称
                            string CustName = dt.Rows[0]["CustName"].ToString().Trim() == "" ? "--" : dt.Rows[0]["CustName"].ToString().Trim();
                            if (setFields.GetsetField("custname", e_CondSetType.fmcstField) == null || setFields.GetsetField("custname", e_CondSetType.fmcstField).Visibled == false)
                            {
                                CustName = "--";
                            }
                            //投诉来源
                            string BY_SoureName = dt.Rows[0]["BY_SoureName"].ToString().Trim() == "" ? "--" : dt.Rows[0]["BY_SoureName"].ToString().Trim();
                            if (setFields.GetsetField("by_soure", e_CondSetType.fmcstField) == null || setFields.GetsetField("by_soure", e_CondSetType.fmcstField).Visibled == false)
                            {
                                BY_SoureName = "--";
                            }
                            sb.Append("<tr><td class='listTitleNew_s' style='text-align:left;' width='17%'>客户名称</td><td class='listNew_s' style='text-align:left;' width='33%'>" + CustName
                                + "</td><td class='listTitleNew_s' style='text-align:left;' width='17%'>投诉来源</td><td class='listNew_s' style='text-align:left;' width='33%'>" + BY_SoureName + "</td></tr>");
                                    
                     
                            //联系手机
                            string BY_Mobile = dt.Rows[0]["BY_Mobile"].ToString().Trim() == "" ? "--" : dt.Rows[0]["BY_Mobile"].ToString().Trim();
                            if (setFields.GetsetField("by_mobile", e_CondSetType.fmcstField) == null || setFields.GetsetField("by_mobile", e_CondSetType.fmcstField).Visibled == false)
                            {
                                BY_Mobile = "--";
                            }
                            //联系电话
                            string BY_ContactPhone = dt.Rows[0]["BY_ContactPhone"].ToString().Trim() == "" ? "--" : dt.Rows[0]["BY_ContactPhone"].ToString().Trim();
                            if (setFields.GetsetField("by_contactphone", e_CondSetType.fmcstField) == null || setFields.GetsetField("by_contactphone", e_CondSetType.fmcstField).Visibled == false)
                            {
                                BY_ContactPhone = "--";
                            }

                            sb.Append("<tr><td class='listTitleNew_s' style='text-align:left;' width='17%'>联系手机</td><td class='listNew_s' style='text-align:left;' width='33%'>" + BY_Mobile
                                    + "</td><td class='listTitleNew_s' style='text-align:left;' width='17%'>联系电话</td><td class='listNew_s' style='text-align:left;' width='33%'>" + BY_ContactPhone + "</td></tr>");
                      

                            //电子邮件
                            string BY_Email = dt.Rows[0]["BY_Email"].ToString().Trim()  == "" ? "--" : dt.Rows[0]["BY_Email"].ToString().Trim();
                            if (setFields.GetsetField("by_email", e_CondSetType.fmcstField) == null || setFields.GetsetField("by_email", e_CondSetType.fmcstField).Visibled == false)
                            {
                                BY_Email = "--";
                            }
                            //投诉次数
                            string BY_InformNum = dt.Rows[0]["BY_InformNum"].ToString().Trim()  == "" ? "--" : dt.Rows[0]["BY_InformNum"].ToString().Trim();
                            if (setFields.GetsetField("by_informnum", e_CondSetType.fmcstField) == null || setFields.GetsetField("by_informnum", e_CondSetType.fmcstField).Visibled == false)
                            {
                                BY_InformNum = "--";
                            }

                            sb.Append("<tr><td class='listTitleNew_s' style='text-align:left;' width='17%'>电子邮件</td><td class='listNew_s' style='text-align:left;' width='33%'>" + BY_Email
                                    + "</td><td class='listTitleNew_s' style='text-align:left;' width='17%'>投诉次数</td><td class='listNew_s' style='text-align:left;' width='33%'>" + BY_InformNum + "</td></tr>");
                   

                            //接收时间
                            string BY_ReceiveTime = dt.Rows[0]["BY_ReceiveTime"].ToString().Trim()  == "" ? "--" : dt.Rows[0]["BY_ReceiveTime"].ToString().Trim();
                            if (setFields.GetsetField("by_receivetime", e_CondSetType.fmcstField) == null || setFields.GetsetField("by_receivetime", e_CondSetType.fmcstField).Visibled == false)
                            {
                                BY_ReceiveTime = "--";
                            }
                            string RegTime = dt.Rows[0]["RegTime"].ToString().Trim() == "" ? "--" : dt.Rows[0]["RegTime"].ToString().Trim();
                            sb.Append("<tr><td class='listTitleNew_s' style='text-align:left;' width='17%'>接收时间</td><td class='listNew_s' style='text-align:left;' width='33%'>" + BY_ReceiveTime

                                    + "</td><td class='listTitleNew_s' style='text-align:left;' width='17%'>登记日期</td><td class='listNew_s' style='text-align:left;' width='33%'>" + RegTime + "</td></tr>");
                        
                     
                        sb.Append("</table>");
                    }
                    Response.Clear();
                    Response.Write(sb.ToString());
                    Response.Flush();
                    Response.End();
                }
            }
            else if (Request.QueryString["Problem_ID"] != null)
            {
                //问题单
                StringBuilder sb = new StringBuilder();
                if (Request.QueryString["Problem_ID"].ToString() != string.Empty)
                {
                    DataTable dt = XmlHttpTable.GETdatetable("Pro_ProblemDeal", " and Problem_ID=" + Request.QueryString["Problem_ID"].ToString());
                    if (dt.Rows.Count > 0)
                    {
                        sb.Append("<table class='listContent' width='380px' >");

                        long lngMessageID = FlowDP.GetMessageId(long.Parse(dt.Rows[0]["flowid"].ToString()), (long)Session["UserID"]);
                        //取得权限控制
                        objFlow oFlow = new objFlow((long)Session["UserID"], long.Parse(dt.Rows[0]["flowModelid"].ToString()), lngMessageID);
                        setFieldCollection setFields = oFlow.setFields;


                        string regUserName = dt.Rows[0]["regUserName"].ToString().Trim() == "" ? "--" : dt.Rows[0]["regUserName"].ToString().Trim();
                        string regDeptname = dt.Rows[0]["regDeptname"].ToString().Trim() == "" ? "--" : dt.Rows[0]["regDeptname"].ToString().Trim();

                        sb.Append("<tr><td class='listTitleNew_s' style='text-align:left;' width='17%'>登记人</td><td class='listNew_s' style='text-align:left;' width='33%'>" + regUserName
                                   + "</td><td class='listTitleNew_s' style='text-align:left;' width='17%'>登记部门</td><td class='listNew_s' style='text-align:left;' width='33%'>" + regDeptname + "</td></tr>");
                       

                        //问题描述
                        string effectid = dt.Rows[0]["effectname"].ToString().Trim() == "" ? "--" : dt.Rows[0]["effectname"].ToString().Trim();
                        if (setFields.GetsetField("effectid", e_CondSetType.fmcstField) == null || setFields.GetsetField("effectid", e_CondSetType.fmcstField).Visibled == false)
                        {
                            effectid = "--";
                        }
                        string instancyid = dt.Rows[0]["instancyname"].ToString().Trim() == "" ? "--" : dt.Rows[0]["instancyname"].ToString().Trim();
                        if (setFields.GetsetField("instancyid", e_CondSetType.fmcstField) == null || setFields.GetsetField("instancyid", e_CondSetType.fmcstField).Visibled == false)
                        {
                            instancyid = "--";
                        }
                        
                        sb.Append("<tr><td class='listTitleNew_s' style='text-align:left;' width='17%'>影响度</td><td class='listNew_s' style='text-align:left;' width='33%'>" + effectid
                                   + "</td><td class='listTitleNew_s' style='text-align:left;' width='17%'>紧急度</td><td class='listNew_s' style='text-align:left;' width='33%'>" + instancyid + "</td></tr>");

                        //问题描述
                        string problem_subject = dt.Rows[0]["problem_subject"].ToString().Trim() == "" ? "--" : dt.Rows[0]["problem_subject"].ToString().Trim();
                        if (setFields.GetsetField("problem_subject", e_CondSetType.fmcstField) == null || setFields.GetsetField("problem_subject", e_CondSetType.fmcstField).Visibled == false)
                        {
                            problem_subject = "--";
                        }
                        string RegTime = dt.Rows[0]["RegTime"].ToString().Trim() == "" ? "--" : dt.Rows[0]["RegTime"].ToString().Trim();


                        sb.Append("<tr><td class='listTitleNew_s' style='text-align:left;' width='17%'>问题描述</td><td class='listNew_s' style='text-align:left;' width='33%'>" + problem_subject
                                   + "</td><td class='listTitleNew_s' style='text-align:left;' width='17%'>登记时间</td><td class='listNew_s' style='text-align:left;' width='33%'>" + RegTime + "</td></tr>");
                        

                        sb.Append("</table>");
                        
                    }
                }

                Response.Clear();
                Response.Write(sb.ToString());
                Response.Flush();
                Response.End();

            }
            else if (Request.QueryString["ServiceLeve"] != null)
            {
                StringBuilder sb = new StringBuilder();
                if (Request.QueryString["ServiceLeve"].ToString() != string.Empty)
                {

                    DataTable dt = XmlHttpTable.GETdatetable("Cst_ServiceLevel", " and ID=" + Request.QueryString["ServiceLeve"].ToString());
                    if (dt.Rows.Count > 0)
                    {
                        sb.Append("<table class='listContent' width='380px' >");
                        if (dt.Rows[0]["LevelName"].ToString().Trim() != "" || dt.Rows[0]["Definition"].ToString().Trim() != "")
                        {
                            sb.Append("<tr><td class='listTitleNew_s' style='text-align:left;' width='17%'>级别名称</td><td class='listNew_s' style='text-align:left;' width='33%'>" + dt.Rows[0]["LevelName"].ToString().Trim()
                                    + "</td><td class='listTitleNew_s' style='text-align:left;' width='17%'>级别定义</td><td class='listNew_s' style='text-align:left;' width='33%'>" + dt.Rows[0]["Definition"].ToString().Trim() + "</td></tr>");
                        }
                        if (dt.Rows[0]["baselevel"].ToString().Trim() != "" || dt.Rows[0]["notinclude"].ToString().Trim() != "")
                        {
                            sb.Append("<tr><td class='listTitleNew_s' style='text-align:left;' width='17%'>服务包括</td><td class='listNew_s' style='text-align:left;' width='33%'>" + dt.Rows[0]["baselevel"].ToString().Trim()
                                    + "</td><td class='listTitleNew_s' style='text-align:left;' width='17%'>服务不包括</td><td class='listNew_s' style='text-align:left;' width='33%'>" + dt.Rows[0]["notinclude"].ToString().Trim() + "</td></tr>");
                        }
                        if (dt.Rows[0]["Availability"].ToString().Trim() != "" || dt.Rows[0]["charge"].ToString().Trim() != "")
                        {
                            sb.Append("<tr><td class='listTitleNew_s' style='text-align:left;' width='17%'>服务有效性</td><td class='listNew_s' style='text-align:left;' width='33%'>" + dt.Rows[0]["Availability"].ToString().Trim()
                                    + "</td><td class='listTitleNew_s' style='text-align:left;' width='17%'>费用说明</td><td class='listNew_s' style='text-align:left;' width='33%'>" + dt.Rows[0]["charge"].ToString().Trim() + "</td></tr>");
                        }

                        sb.Append("</table>");
                    }
                    Response.Clear();
                    Response.Write(sb.ToString());
                    Response.Flush();
                    Response.End();
                }
            }
            else if (Request.QueryString["V_EquChange"] != null)
            {
                StringBuilder sb = new StringBuilder();
                if (Request.QueryString["V_EquChange"].ToString() != string.Empty)
                {

                    DataTable dt = XmlHttpTable.GETdatetableChane( Request.QueryString["V_EquChange"].ToString());
                    if (dt.Rows.Count > 0)
                    {
                        //获取messageid
                        long lngMessageID = FlowDP.GetMessageId(long.Parse(dt.Rows[0]["flowid"].ToString()), (long)Session["UserID"]);
                        //取得权限控制
                        objFlow oFlow = new objFlow((long)Session["UserID"], long.Parse(dt.Rows[0]["flowModelid"].ToString()), lngMessageID);
                        setFieldCollection setFields = oFlow.setFields;

                        string strCustName = string.Empty;
                        string strCustAddress = string.Empty;
                        string strcontact = string.Empty;
                        string strctel = string.Empty;
                        string strCustCode = string.Empty;
                        string strEmail = string.Empty;
                        string strMastCust = string.Empty;
                        string strjob = string.Empty;
                        if (setFields.GetsetField("custinfo", e_CondSetType.fmcstField) == null || setFields.GetsetField("custinfo", e_CondSetType.fmcstField).Visibled == false)
                        {
                            strCustName = "--";
                            strCustAddress = "--";
                            strcontact = "--";
                            strctel = "--";
                            strCustCode = "--";
                            strEmail = "--";
                            strMastCust = "--";
                        }
                        else
                        {
                            strCustName = dt.Rows[0]["ShortName"].ToString().Trim() == "" ? "--" : dt.Rows[0]["ShortName"].ToString().Trim();
                            strCustAddress = dt.Rows[0]["Address"].ToString().Trim() == "" ? "--" : dt.Rows[0]["Address"].ToString().Trim();
                            strcontact = dt.Rows[0]["LinkMan1"].ToString().Trim() == "" ? "--" : dt.Rows[0]["LinkMan1"].ToString().Trim();
                            strctel = dt.Rows[0]["Tel1"].ToString().Trim() == "" ? "--" : dt.Rows[0]["Tel1"].ToString().Trim();
                            strCustCode = dt.Rows[0]["CustDeptName"].ToString().Trim() == "" ? "--" : dt.Rows[0]["CustDeptName"].ToString().Trim();
                            strEmail = dt.Rows[0]["Email"].ToString().Trim() == "" ? "--" : dt.Rows[0]["Email"].ToString().Trim();
                            strMastCust = dt.Rows[0]["mastcustname"].ToString().Trim() == "" ? "--" : dt.Rows[0]["mastcustname"].ToString().Trim();
                            strjob = dt.Rows[0]["job"].ToString().Trim() == "" ? "--" : dt.Rows[0]["job"].ToString().Trim();
                        }
               
                        sb.Append("<table class='listContent' width='420px' >");

                        sb.Append("<tr><td class='listTitleNew_s' style='text-align:left;' width='17%'>用户名称</td><td class='listNew_s' style='text-align:left;' width='33%'>" + strCustName
                                + "</td><td class='listTitleNew_s' style='text-align:left;' width='17%'>用户地址</td><td class='listNew_s' style='text-align:left;' width='33%'>" + strCustAddress + "</td></tr>");

                        sb.Append("<tr><td class='listTitleNew_s' style='text-align:left;' width='17%'>联系人</td><td class='listNew_s' style='text-align:left;' width='33%'>" + strcontact
                                + "</td><td class='listTitleNew_s' style='text-align:left;' width='17%'>联系电话</td><td class='listNew_s' style='text-align:left;' width='33%'>" + strctel + "</td></tr>");

                        sb.Append("<tr><td class='listTitleNew_s' style='text-align:left;' width='17%'>用户部门</td><td class='listNew_s' style='text-align:left;' width='33%'>" + strCustCode
                                + "</td><td class='listTitleNew_s' style='text-align:left;' width='17%'>电子邮件</td><td class='listNew_s' style='text-align:left;' width='33%'>" + strEmail + "</td></tr>");

                        sb.Append("<tr><td class='listTitleNew_s' style='text-align:left;' width='17%'>服务单位</td><td class='listNew_s' style='text-align:left;' width='33%'>" + strMastCust
                                + "</td><td class='listTitleNew_s' style='text-align:left;' width='17%'>职位</td><td class='listNew_s' style='text-align:left;' width='33%'>" + strjob + "</td></tr>");


                        DataTable dt2 = ZHServiceDP.GetCurrDealName(long.Parse(dt.Rows[0]["FlowID"].ToString()));
                        string sCurrNode = string.Empty;
                        string sCurrPerson = string.Empty;
                        if (dt2.Rows.Count > 0)
                        {
                            for (int j = 0; j < dt2.Rows.Count; j++)
                            {
                                if (j == dt2.Rows.Count - 1)
                                {
                                    sCurrNode = dt2.Rows[j]["NodeName"].ToString();
                                    sCurrPerson = dt2.Rows[j]["Name"].ToString();
                                }
                                else
                                {
                                    sCurrNode = dt2.Rows[j]["NodeName"].ToString() + ",";
                                    sCurrPerson = dt2.Rows[j]["Name"].ToString() + ",";
                                }
                            }
                        }

                        sCurrPerson = sCurrPerson == "" ? "--" : sCurrPerson;
                        sCurrNode = sCurrNode == "" ? "--" : sCurrNode;
                        sb.Append("<tr><td class='listTitleNew_s' style='text-align:left;' width='17%'>当前处理人</td><td class='listNew_s' style='text-align:left; color:Red;' width='33%'>" + sCurrPerson
                                  + "</td><td class='listTitleNew_s' style='text-align:left;' width='17%'>当前环节</td><td class='listNew_s' style='text-align:left; color:Red;' width='33%'>" + sCurrNode + "</td></tr>");
                        sb.Append("</table>");
                    }
                    Response.Clear();
                    Response.Write(sb.ToString());
                    Response.Flush();
                    Response.End();
                }
            }
            else if (Request.QueryString["DemandID"] != null) //需求管理
            {
                StringBuilder sb = new StringBuilder();
                if (Request.QueryString["DemandID"].ToString() != string.Empty)
                {
                    DataTable dt = XmlHttpTable.GETdatetable("req_demand", " and ID=" + Request.QueryString["DemandID"].ToString());
                    if (dt.Rows.Count > 0)
                    {
                        sb.Append("<table class='listContent' width='380px' >");                       

                        string regUserName = dt.Rows[0]["RegUserName"].ToString().Trim() == "" ? "--" : dt.Rows[0]["RegUserName"].ToString().Trim();
                        string regDeptname = dt.Rows[0]["RegDeptName"].ToString().Trim() == "" ? "--" : dt.Rows[0]["RegDeptName"].ToString().Trim();
                        string strCustName = dt.Rows[0]["CustUserName"].ToString();
                        string strCustTel = dt.Rows[0]["CustTel"].ToString();
                        string strCustEmail = dt.Rows[0]["CustEmail"].ToString();
                        string strEquipmentName = dt.Rows[0]["EquipmentName"].ToString();
                        string strDemandTypeName = dt.Rows[0]["DemandTypeName"].ToString();
                        string strDemandSubject = dt.Rows[0]["DemandSubject"].ToString();
                        string strDemandStatus = dt.Rows[0]["DemandStatus"].ToString();


                        sb.Append("<tr><td class='listTitleNew_s' style='text-align:left;' width='17%'>客户名称</td><td class='listNew_s' style='text-align:left;' width='33%'>" + strCustName
                                    + "</td><td class='listTitleNew_s' style='text-align:left;' width='17%'>联系电话</td><td class='listNew_s' style='text-align:left;' width='33%'>" + strCustTel + "</td></tr>");

                        sb.Append("<tr><td class='listTitleNew_s' style='text-align:left;' width='17%'>客户邮箱</td><td class='listNew_s' style='text-align:left;' width='33%'>" + strCustEmail
                                    + "</td><td class='listTitleNew_s' style='text-align:left;' width='17%'>资产名称</td><td class='listNew_s' style='text-align:left;' width='33%'>" + strEquipmentName + "</td></tr>");

                        sb.Append("<tr><td class='listTitleNew_s' style='text-align:left;' width='17%'>登单人</td><td class='listNew_s' style='text-align:left;' width='33%'>" + regUserName
                                   + "</td><td class='listTitleNew_s' style='text-align:left;' width='17%'>登单部门</td><td class='listNew_s' style='text-align:left;' width='33%'>" + regDeptname + "</td></tr>");

                        sb.Append("<tr><td class='listTitleNew_s' style='text-align:left;' width='17%'>需求类别</td><td class='listNew_s' style='text-align:left;' width='33%'>" + strDemandTypeName
                                   + "</td><td class='listTitleNew_s' style='text-align:left;' width='17%'>需求状态</td><td class='listNew_s' style='text-align:left;' width='33%'>" + strDemandStatus + "</td></tr>");

                        sb.Append("<tr><td class='listTitleNew_s' style='text-align:left;' width='17%'>需求主题</td><td class='listNew_s' style='text-align:left;' width='33%' colspan='3'>" + strDemandSubject
                                   +"</tr>");

                        sb.Append("</table>");

                    }
                }

                Response.Clear();
                Response.Write(sb.ToString());
                Response.Flush();
                Response.End();
            }

        }
    }

}

