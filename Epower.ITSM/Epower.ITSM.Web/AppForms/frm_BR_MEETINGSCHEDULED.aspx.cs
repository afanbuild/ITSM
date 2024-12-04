using System;
using System.Collections;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Web.UI.HtmlControls;
using System.Xml.Linq;
using EpowerCom;
using System.Xml;
using appDataProcess;
using Epower.ITSM.Base;
using Epower.DevBase.Organization.SqlDAL;
using Epower.DevBase.BaseTools;
using Epower.ITSM.SqlDAL;

namespace Epower.ITSM.Web.AppForms
{
    public partial class frm_BR_MEETINGSCHEDULED : BasePage
    {
        #region 变量
        private FlowForms myFlowForms;
        #endregion

        #region 窗体加载
        /// <summary>
        /// 窗体加载
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void Page_Load(object sender, EventArgs e)
        {
            //实例化类
            myFlowForms = (FlowForms)this.Master;

            //设置只读
            myFlowForms.mySetContentReadOnly += new FlowForms.DoContentActions(Master_mySetContentReadOnly);
            //取得页面控件的值，组成XML对象
            myFlowForms.myGetFormsValue += new FlowForms.GetFormsValue(Master_myGetFormsValue);
            //设置页面值,根据流程节点设置页面控件只读性

            myFlowForms.mySetFormsValue += new FlowForms.DoContentActions(myFlowForms_mySetFormsValue);
            myFlowForms.myPreClickCustomize += new FlowForms.DoContentSubmitValid(myFlowForms_myPreClickCustomize);
            myFlowForms.myPreSaveClickCustomize += new FlowForms.DoContentValid(Master_myPreSaveClickCustomize);

            if (!IsPostBack)
            {

                this.ctrStartTime.dateTime = DateTime.Now;
                this.ctrEndTime.dateTime = DateTime.Now;
            }
        }
        #endregion

        #region 设置只读 Master_mySetContentReadOnly
        /// <summary>
        /// 设置只读
        /// </summary>
        void Master_mySetContentReadOnly()
        {
            //会议名称
            if (CtrMeetingName.ContralState != eOA_FlowControlState.eHidden)
                CtrMeetingName.ContralState = eOA_FlowControlState.eReadOnly;

            //议题
            if (CtrTitle.ContralState != eOA_FlowControlState.eHidden)
                CtrTitle.ContralState = eOA_FlowControlState.eReadOnly;


            //地址
            if (CtrAddress.ContralState != eOA_FlowControlState.eHidden)
                CtrAddress.ContralState = eOA_FlowControlState.eReadOnly;


            //部门
            if (CtrDepartmentName.ContralState != eOA_FlowControlState.eHidden)
                CtrDepartmentName.ContralState = eOA_FlowControlState.eReadOnly;


            //会议室
            if (ctrdropMeetingRoom.ContralState != eOA_FlowControlState.eHidden)
                ctrdropMeetingRoom.ContralState = eOA_FlowControlState.eReadOnly;

            //预定日期
            if (Ctrdatetime.ContralState != eOA_FlowControlState.eHidden)
                Ctrdatetime.ContralState = eOA_FlowControlState.eReadOnly;

            //开始时间
            if (ctrStartTime.ContralState != eOA_FlowControlState.eHidden)
                ctrStartTime.ContralState = eOA_FlowControlState.eReadOnly;


            //结束时间
            if (ctrEndTime.ContralState != eOA_FlowControlState.eHidden)
                ctrEndTime.ContralState = eOA_FlowControlState.eReadOnly;

            //主持人
            if (CtrHostNames.ContralState != eOA_FlowControlState.eHidden)
                CtrHostNames.ContralState = eOA_FlowControlState.eReadOnly;

            //电话
            if (CtrPhone.ContralState != eOA_FlowControlState.eHidden)
                CtrPhone.ContralState = eOA_FlowControlState.eReadOnly;

            //服务
            if (cheService.Visible)
                cheService.Enabled = false;


            //备注
            if (CtrRemarKs.ContralState != eOA_FlowControlState.eHidden)
                CtrRemarKs.ContralState = eOA_FlowControlState.eReadOnly;


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
            FieldValues fv = new FieldValues();
            ///注意：当前应用没有附属表，当有附属表的情况，附属表的信息项值的XML存到 Tables节点里          

            #region 会议名称
            fv.Add("MeetingName", this.CtrMeetingName.Value.ToString());
            #endregion

            #region 议题
            fv.Add("Title", this.CtrTitle.Value.ToString());
            #endregion

            #region 地点
            fv.Add("Address", this.CtrAddress.Value.ToString());
            #endregion

            #region 会议室
            fv.Add("MeetingID", this.ctrdropMeetingRoom.CatelogID.ToString());
            fv.Add("MeetingRoom", this.ctrdropMeetingRoom.CatelogValue.ToString());
            #endregion

            #region 部门
            fv.Add("DepartmentID", this.CtrDepartmentName.DeptID.ToString());
            fv.Add("DepartmentName", this.CtrDepartmentName.DeptName.ToString());
            #endregion

            #region 主持人
            fv.Add("HostID", this.CtrHostNames.UserID.ToString());
            fv.Add("HostName", this.CtrHostNames.UserName.ToString());
            #endregion

            #region 开始时间
            fv.Add("StartTime", ctrStartTime.dateTime.ToString());
            #endregion

            #region 结束时间
            fv.Add("EndTime", ctrEndTime.dateTime.ToString());
            #endregion

            #region 预定日期
            fv.Add("datetime", ctrStartTime.dateTime.Date.ToString());
            #endregion

            #region 联系电话
            fv.Add("Phone", this.CtrPhone.Value.ToString());
            #endregion

            #region 服务
            string service = "";
            for (int i = 0; i < this.cheService.Items.Count; i++)
            {
                if (this.cheService.Items[i].Selected == true)
                {
                    service += this.cheService.Items[i].Value + ",";
                }
            }
            if (service.LastIndexOf(",") == service.Length - 1)
            {
                if (service.Length - 1 >= 0)
                {
                    service = service.Substring(0, service.Length - 1);
                }
            }

            fv.Add("Service", service);
            #endregion

            #region 备注
            fv.Add("RemarKs", this.CtrRemarKs.Value.ToString());
            #endregion

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

            myFlowForms.FormTitle = oFlow.FlowName;

            if (!string.IsNullOrEmpty(oFlow.FlowID.ToString()))
            {
                if (oFlow.FlowID.ToString() != "0")
                {
                    this.hidDeptID.Value = oFlow.FlowID.ToString();
                }
            }

            #region 设置页面值

            DataTable dt = null;
            if (oFlow.MessageID != 0)
            {
                ImplDataProcess dp = new ImplDataProcess(oFlow.AppID);

                #region 查询
                DataSet ds = dp.GetFieldsDataSet(oFlow.FlowID, oFlow.OpID);
                dt = ds.Tables[0];
                #endregion
            }

            if (dt != null)          //如果为修改，赋值
            {
                if (dt.Rows.Count > 0)
                {
                    DataRow row = dt.Rows[0];


                    #region 为控件赋值
                    CtrMeetingName.Value = row["MeetingName"].ToString();//会议名称
                    CtrTitle.Value = row["Title"].ToString();//议题
                    CtrAddress.Value = row["Address"].ToString();//地点

                    ctrdropMeetingRoom.CatelogID = long.Parse(row["MeetingID"].ToString() == "" ? "0" : row["MeetingID"].ToString());//会议室ID
                    ctrdropMeetingRoom.CatelogValue = row["MeetingRoom"].ToString();//会议室


                    string departid = row["DepartmentID"].ToString();//部门ID
                    string departname = row["DepartmentName"].ToString();//部门
                    CtrDepartmentName.DeptID = long.Parse(departid);
                    CtrDepartmentName.DeptName = departname;

                    CtrHostNames.UserID = long.Parse(row["HostID"].ToString() == "" ? "0" : row["HostID"].ToString());//主持人ID
                    CtrHostNames.UserName = row["HostName"].ToString();//主持人


                    Ctrdatetime.dateTime = Convert.ToDateTime(row["datetime"].ToString());//预定日期

                    ctrStartTime.dateTime = Convert.ToDateTime(row["StartTime"].ToString());//开始时间
                    ctrEndTime.dateTime = Convert.ToDateTime(row["EndTime"].ToString());//结束时间


                    string phone = row["Phone"].ToString();//联系电话
                    CtrPhone.Value = phone;

                    #region 服务
                    if (!string.IsNullOrEmpty(row["Service"].ToString()))
                    {
                        string servicevalue = row["Service"].ToString();//服务
                        if (servicevalue.IndexOf(',') > 0)
                        {
                            string[] strs = servicevalue.Split(',');

                            for (int i = 0; i < strs.Length; i++)
                            {
                                cheService.Items[Convert.ToInt32(strs[i])].Selected = true; ;
                            }
                        }
                    }
                    #endregion

                    CtrRemarKs.Value = row["RemarKs"].ToString();//备注
                    #endregion
                }
                else
                {
                    #region 初始化赋值 yxq

                    //主持人
                    CtrHostNames.UserID = long.Parse(Session["UserID"].ToString());
                    CtrHostNames.UserName = Session["PersonName"].ToString();

                    //预定日期
                    Ctrdatetime.dateTime = DateTime.Now;
                    #endregion
                }
            }
            else
            {
                #region 初始化赋值 yxq
                //主持人
                CtrHostNames.UserID = long.Parse(Session["UserID"].ToString());
                CtrHostNames.UserName = Session["PersonName"].ToString();

                //预定日期
                Ctrdatetime.dateTime = DateTime.Now;
                #endregion
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
                    //会议名称
                    case "meetingname":
                        CtrMeetingName.ContralState = eOA_FlowControlState.eReadOnly;
                        if (sf.Visibled == false)
                        {
                            CtrMeetingName.ContralState = eOA_FlowControlState.eHidden;
                        }
                        break;
                    //议题
                    case "title":
                        CtrTitle.ContralState = eOA_FlowControlState.eReadOnly;
                        if (sf.Visibled == false)
                        {
                            CtrTitle.ContralState = eOA_FlowControlState.eHidden;
                        }
                        break;
                    //地址
                    case "address":
                        CtrAddress.ContralState = eOA_FlowControlState.eReadOnly;
                        if (sf.Visibled == false)
                        {
                            CtrAddress.ContralState = eOA_FlowControlState.eHidden;
                        }
                        break;
                    //会议室
                    case "meetingroom":
                        ctrdropMeetingRoom.ContralState = eOA_FlowControlState.eReadOnly;
                        if (sf.Visibled == false)
                        {
                            ctrdropMeetingRoom.ContralState = eOA_FlowControlState.eHidden;
                        }
                        break;
                    //部门
                    case "departmentname":
                        CtrDepartmentName.ContralState = eOA_FlowControlState.eReadOnly;
                        if (sf.Visibled == false)
                        {
                            CtrDepartmentName.ContralState = eOA_FlowControlState.eHidden;
                        }
                        break;
                    //主持人
                    case "hostname":
                        CtrHostNames.ContralState = eOA_FlowControlState.eReadOnly;
                        if (sf.Visibled == false)
                        {
                            CtrHostNames.ContralState = eOA_FlowControlState.eHidden;
                        }
                        break;
                    //预定日期
                    case "datetime":
                        Ctrdatetime.ContralState = eOA_FlowControlState.eReadOnly;
                        if (sf.Visibled == false)
                        {
                            Ctrdatetime.ContralState = eOA_FlowControlState.eHidden;
                        }
                        break;
                    //开始时间
                    case "starttime":
                        ctrStartTime.ContralState = eOA_FlowControlState.eReadOnly;
                        if (sf.Visibled == false)
                        {
                            ctrStartTime.ContralState = eOA_FlowControlState.eHidden;
                        }
                        break;
                    //结束时间
                    case "endtime":
                        ctrEndTime.ContralState = eOA_FlowControlState.eReadOnly;
                        if (sf.Visibled == false)
                        {
                            ctrEndTime.ContralState = eOA_FlowControlState.eHidden;
                        }
                        break;
                    //联系电话
                    case "phone":
                        CtrPhone.ContralState = eOA_FlowControlState.eReadOnly;
                        if (sf.Visibled == false)
                        {
                            CtrPhone.ContralState = eOA_FlowControlState.eHidden;
                        }
                        break;
                    //服务
                    case "service":
                        cheService.Enabled = false;
                        if (sf.Visibled == false)
                        {
                            cheService.Visible = false;
                        }
                        break;
                    //备注
                    case "remarks":
                        CtrRemarKs.ContralState = eOA_FlowControlState.eReadOnly;
                        if (sf.Visibled == false)
                        {
                            CtrRemarKs.ContralState = eOA_FlowControlState.eHidden;
                        }
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
            bool flag = true;

            

            //第一次 起草的时候，进行判断，之后就不进行判断了。
            if (string.IsNullOrEmpty(this.hidDeptID.Value.ToString()) || this.hidDeptID.Value.ToString() == "0")
            {
                if (this.ctrStartTime.dateTime.CompareTo(DateTime.Now) < 0)
                {
                    PageTool.MsgBox(this, "申请时间不能早于当前时间!");
                    flag = false;
                }

                DateTime startime = this.ctrStartTime.dateTime;
                DateTime endtime = this.ctrEndTime.dateTime;

                if (startime.CompareTo(endtime) >= 0)
                {
                    PageTool.MsgBox(this, "结束时间必须晚于开始时间!");
                    flag = false;
                }

                if (startime.Date.ToString() != endtime.Date.ToString())
                {
                    PageTool.MsgBox(this, "开始时间和结束时间必须在同一天内!");
                    flag = false;
                }

                #region 查询历史记录，看此时间段和会议室是否被占用
                DataTable dt = App_pub_BR_MEETINGSCHEDULED_DP.Get_Meeting_Scheduled(startime.ToString(), endtime.ToString(), ctrdropMeetingRoom.CatelogID);

                if (dt != null && dt.Rows.Count > 0)
                {
                    if (!string.IsNullOrEmpty(dt.Rows[0][0].ToString()))
                    {
                        int num = Convert.ToInt32(dt.Rows[0][0].ToString());
                        if (num >= 1)
                        {
                            PageTool.MsgBox(this, "此时间段会议室已经被占用，请另选时间申请!");
                            flag = false;
                        }
                    }
                }
                #endregion

            }
            return flag;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        bool Master_myPreSaveClickCustomize()
        {
            return true;
        }
        #endregion

        #region 公共函数
        /// <summary>
        /// 处理空值
        /// </summary>
        /// <param name="str"></param>
        /// <returns></returns>
        private string SpecTransText(string str)
        {
            string strR = str;
            if (str == "")
            {
                strR = "--";
            }
            if (str == "--")
            {
                strR = "";
            }
            return strR;
        }
        #endregion
    }
}
