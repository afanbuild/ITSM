/***************************************************************************
 * desc：自定义表单流程表单页面
 * 
 * 
 * 
 * Create By:zhumingchun
 * Create Date:2011-08-17
 * ************************************************************************/

using System;
using System.Data;
using System.Configuration;
using System.Collections;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Web.UI.HtmlControls;

//引用
using System.Xml;
using appDataProcess;
using Epower.DevBase.Organization.SqlDAL;
using Epower.DevBase.Organization.Base;
using EpowerCom;
using EpowerGlobal;
using Epower.DevBase.BaseTools;
using Epower.ITSM.SqlDAL;
using Epower.ITSM.Base;

namespace Epower.ITSM.Web.AppForms
{
    public partial class frm_UserDefine : BasePage
    {
        #region 变量定义
        /// <summary>
        /// 母版页的定义变量
        /// </summary>
        private FlowFormsUserDefine myFlowForms;
        #endregion 

        #region 属性定义
        /// <summary>
        /// 表单类型
        /// </summary>
        protected string sDjlx
        {
            get
            {
                if (ViewState["djlx"] != null)
                {
                    return ViewState["djlx"].ToString();
                }
                else
                {
                    return "TT";
                }
            }
            set
            {
                ViewState["djlx"] = value;
            }
        }
        /// <summary>
        /// 表单sn
        /// </summary>
        protected string sDjsn
        {
            get
            {
                if (ViewState["djsn"] != null)
                {
                    return ViewState["djsn"].ToString();
                }
                else
                {
                    return "0";
                }
            }
            set
            {
                ViewState["djsn"] = value;
            }
        }
        /// <summary>
        /// 表单高度
        /// </summary>
        protected string sDjHeight
        {
            get
            {
                if (ViewState["djHeight"] != null)
                {
                    return ViewState["djHeight"].ToString();
                }
                else
                {
                    return "300";
                }
            }
            set
            {
                ViewState["djHeight"] = value;
            }
        }
        /// <summary>
        /// 流程唯一编号
        /// </summary>
        protected string sID
        {
            get
            {
                if (ViewState["id"] != null)
                {
                    return ViewState["id"].ToString();
                }
                else
                {
                    return "0";
                }
            }
            set
            {
                ViewState["id"] = value;
            }
        }
        /// <summary>
        /// 流程信息控制
        /// </summary>
        protected string sInfControl
        {
            get
            {
                if (ViewState["InfControl"] != null)
                {
                    return ViewState["InfControl"].ToString();
                }
                else
                {
                    return "false";
                }
            }
            set
            {
                ViewState["InfControl"] = value;
            }
        }
        #endregion 

        #region 页面加载 Page_Load
        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void Page_Load(object sender, EventArgs e)
        {
            //实例化类
            myFlowForms = (FlowFormsUserDefine)this.Master;

            //设置只读
            myFlowForms.mySetContentReadOnly += new FlowFormsUserDefine.DoContentActions(Master_mySetContentReadOnly);
            //取得页面控件的值，组成XML对象
            myFlowForms.myGetFormsValue += new FlowFormsUserDefine.GetFormsValue(Master_myGetFormsValue);
            //设置页面值,根据流程节点设置页面控件只读性
            myFlowForms.mySetFormsValue += new FlowFormsUserDefine.DoContentActions(myFlowForms_mySetFormsValue);
            //流程提交前执行
            myFlowForms.myPreClickCustomize += new FlowFormsUserDefine.DoContentSubmitValid(myFlowForms_myPreClickCustomize);
            //流程暂存前执行
            myFlowForms.myPreSaveClickCustomize += new FlowFormsUserDefine.DoContentValid(Master_myPreSaveClickCustomize);
            if (Page.IsPostBack == false)
            {
                hidUser.Value = Session["UserName"].ToString();
            }
        }
        #endregion 

        #region 设置只读 Master_mySetContentReadOnly
        /// <summary>
        /// 设置只读
        /// </summary>
        void Master_mySetContentReadOnly()
        {
            sInfControl = "true";
        }
        #endregion

        #region 取得页面控件值，组成XML对象 Master_myGetFormsValue
        /// <summary>
        /// 取得页面控件值，组成XML对象
        /// </summary>
        /// <param name="lngActionID"></param>
        /// <param name="strActionName"></param>
        /// <returns></returns>
        private XmlDocument Master_myGetFormsValue(long lngActionID, string strActionName)
        {
            ///注意：当前应用没有附属表，当有附属表的情况，附属表的信息项值的XML存到 Tables节点里
            FieldValues fv = new FieldValues();
            fv.Add("ID", sID);
            XmlDocument xmlDoc = fv.GetXmlObject();
            return xmlDoc;
        }
        #endregion

        #region 设置页面值,根据流程节点设置页面控件只读性 Master_mySetFormsValue
        /// <summary>
        /// 设置页面值,根据流程节点设置页面控件只读性
        /// </summary>
        private void myFlowForms_mySetFormsValue()
        {
            objFlow oFlow = myFlowForms.oFlow;                //工作流对象
            myFlowForms.FormTitle = oFlow.FlowName.Trim();
            #region 设置页面值
            DataTable dt = FC_BILLZLDP.GetUserDefineTable(oFlow.FlowModelID);   //取得自定义表单值
            foreach (DataRow dr in dt.Rows)
            {
                sDjlx = dr["djlx"].ToString();   //表单分类
                sDjsn = dr["djsn"].ToString();   //表单sn

                string sdjposition = dr["djposition"].ToString();
                string[] sarr = sdjposition.Split(',');
                sDjHeight = sarr[3].ToString();
            }

            if (oFlow.MessageID != 0)  //有数据，取编号ID
            {
                sID = FC_BILLZLDP.GetFlowUserDifineID(oFlow.FlowID);
            }
            else   //新增，生成编号
            {
                sID = EpowerGlobal.EPGlobal.GetNextID("FlowUserDefineID").ToString(); ;
            }
            #endregion

            #region 根据流程设置页面控件是否只读
            setFieldCollection setFields = oFlow.setFields;
            foreach (setField sf in setFields)
            {
                if (sf.Editable == true && sf.Visibled == true)
                    continue;
                switch (sf.Name.ToLower())
                {
                    case "infcontrol":
                        sInfControl = "true";
                        break;
                    default:
                        break;
                }
            }
            #endregion
        }
        #endregion

        #region 保存流程之前执行代码
        /// <summary>
        /// 保存流程之前执行代码
        /// </summary>
        /// <returns></returns>
        bool myFlowForms_myPreClickCustomize(long lngActionID, string strActionName)
        {
            return true;
        }

        /// <summary>
        /// 暂存流程之前执行代码
        /// </summary>
        /// <returns></returns>
        bool Master_myPreSaveClickCustomize()
        {
            return true;
        }
        #endregion
    }
}
