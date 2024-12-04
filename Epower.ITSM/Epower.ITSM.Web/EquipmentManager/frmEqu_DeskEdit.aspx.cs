/****************************************************************************
 * 
 * description:设备资料维护
 * 
 * 
 * 
 * Create by:
 * Create Date:2007-10-04
 * *************************************************************************/

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
using System.Xml;
using System.Drawing;
using System.Text;

using Epower.ITSM.SqlDAL;
using Epower.ITSM.Base;
using Epower.DevBase.Organization.SqlDAL;
using EpowerCom;
using EpowerGlobal;
using Epower.DevBase.BaseTools;
using Epower.ITSM.SqlDAL.ES_TBLCS;
using System.Collections.Generic;

namespace Epower.ITSM.Web.EquipmentManager
{
    public partial class frmEqu_DeskEdit : BasePage
    {
        string sSchemaValue = "";
        protected long lngCatalogID = 0;

        string sHistorySchema = "";   //历史版本的schema 仅用于历史版本展示
        #region 获得传过来的参数用来判断父窗体的控件id的名称的相同部分
        /// <summary>
        /// 获得传过来的参数用来判断父窗体的控件id的名称的相同部分
        /// </summary>
        public string Opener_ClientId
        {

            get
            {
                return (Request["Opener_ClientId"] == null) ? "" : Request["Opener_ClientId"].ToString().Replace("hidClientId_ForOpenerPage", "");
            }
        }
        #endregion

        #region 属性


        /// <summary>
        /// 是否来源于图形展示详细
        /// </summary>
        protected bool ShowDetail
        {
            get { if (Request["ShowDetail"] != null) return true; else return false; }
        }

        /// <summary>
        /// 配置基线图上的版本
        /// </summary>
        protected long ChartVersion
        {
            get
            {
                if (Request["Version"] != null && Request["Version"].ToString() != "") return long.Parse(Request["Version"].ToString()); else return -1;
            }
        }
        /// <summary>
        /// 传过来的参数判断哪个页面传过来的
        /// </summary>
        public string TypeFrm
        {
            get
            {

                if (Request.QueryString["TypeFrm"] != null)
                {
                    return Request.QueryString["TypeFrm"];
                }
                else
                {
                    return "";
                }
            }
        }




        /// <summary>
        /// 是否来源于选择
        /// </summary>
        protected bool IsSelect
        {
            get { if (Request["IsSelect"] != null) return true; else return false; }
        }

        /// <summary>
        /// 变更单上的FlowID
        /// </summary>
        protected long ChangeFlowID
        {
            get { if (Request["ChangeBillFlowID"] != null) return long.Parse(Request["ChangeBillFlowID"].ToString()); else return 0; }
        }

        /// <summary>
        /// 是否打开新窗口,新窗口返回时关闭
        /// </summary>
        protected bool IsNewWin
        {
            get { if (Request["newWin"] != null) return true; else return false; }
        }

        /// <summary>
        /// 是否来源于查看相同配置
        /// </summary>
        protected bool IsSameSchemaItem
        {
            get { if (Request["IsSameSchemaItem"] != null) return true; else return false; }
        }


        protected string EquID
        {
            get
            {
                if (this.Master.MainID.Trim() != string.Empty)
                {
                    return this.Master.MainID.Trim();
                }
                else
                {
                    return "0";
                }
            }

        }

        /// <summary>
        /// 设备分类ID
        /// </summary>
        protected string CatalogID
        {
            get
            {
                return CtrEquCataDropList1.CatelogID.ToString();
            }
        }
        /// <summary>
        /// 设备分类名称
        /// </summary>
        protected string CatalogName
        {
            get
            {
                return CtrEquCataDropList1.CatelogValue.Trim();
            }
        }
        /// <summary>
        /// 设备分类名称
        /// </summary>
        protected string FullID
        {
            get
            {
                return Equ_SubjectDP.GetSubjectFullID(long.Parse(CatalogID));
            }
        }
        /// <summary>
        /// 是否从变更单过来
        /// </summary>
        protected bool IsChange
        {
            get
            {
                if (Request["IsChange"] != null)
                    return true;
                else
                    return false;
            }
        }

        /// <summary>
        /// 是否从变更单过来进入编辑状态
        /// </summary>
        protected bool IsChangeEdit
        {
            get
            {
                if (Request["IsChEdit"] != null)
                    return true;
                else
                    return false;
            }
        }

        /// <summary>
        /// 是否从关联资产过来
        /// </summary>
        protected bool IsRel
        {
            get
            {
                if (Request["IsRel"] != null)
                    return true;
                else
                    return false;
            }
        }
        #endregion

        #region SetParentButtonEvent
        /// <summary>
        /// 
        /// </summary>
        protected void SetParentButtonEvent()
        {
            if (IsChange) //从变更单过来的
            {
                //变更单过来不检查权限
                this.Master.OperatorID = 0;
            }
            else
            {
                this.Master.OperatorID = Constant.EquManager;
            }
            this.Master.IsCheckRight = true;

            if (this.Request.QueryString["id"] != null)
            {
                this.Master.MainID = this.Request.QueryString["id"].ToString();
            }
            this.Master.Master_Button_New_Click += new Global_BtnClick(Master_Master_Button_New_Click);
            this.Master.Master_Button_Delete_Click += new Global_BtnClick(Master_Master_Button_Delete_Click);
            this.Master.Master_Button_Save_Click += new Global_BtnClick(Master_Master_Button_Save_Click);
            this.Master.Master_Button_GoHistory_Click += new Global_BtnClick(Master_Master_Button_GoHistory_Click);
            this.Master.DefaultButton = "btn_save";
            if (string.IsNullOrEmpty(this.Master.MainID.ToString()))
                this.Master.ShowAddPageButton();
            else
                this.Master.ShowEditPageButton();

            if (Request["FlowID"] != null)   //如果从服务单过来的，只读
            {
                this.Master.ShowNewButton(false);
                this.Master.ShowDeleteButton(false);
                this.Master.ShowSaveButton(false);
                SetFormReadOnly();
            }
            if (IsChange) //从变更单过来的
            {
                this.Master.ShowNewButton(false);
                this.Master.ShowDeleteButton(false);
                if (IsChangeEdit == true)
                {
                    this.Master.ShowSaveButton(true);
                }
                else
                {
                    this.Master.ShowSaveButton(false);
                    SetFormReadOnly();
                }
            }
            if (Master.GetEditRight() == false && this.Request.QueryString["id"] != null)
            {
                this.Master.ShowNewButton(false);
                this.Master.ShowDeleteButton(false);
                this.Master.ShowSaveButton(false);
            }
            if (Request["Page"] == "IssueBase")
            {
                trType.Visible = true;
            }
        }
        #endregion

        #region Master_Master_Button_New_Click
        /// <summary>
        /// 
        /// </summary>
        void Master_Master_Button_New_Click()
        {
            Response.Redirect("frmEqu_DeskEdit.aspx?subjectid=" + CatalogID);
        }
        #endregion

        #region Master_Master_Button_Delete_Click
        /// <summary>
        /// 
        /// </summary>
        void Master_Master_Button_Delete_Click()
        {
            if (!String.IsNullOrEmpty(this.Master.MainID.ToString()))
            {
                Equ_DeskDP ee = new Equ_DeskDP();
                ee.DeleteRecorded(long.Parse(this.Master.MainID.Trim()));

                //删除自定义项
                //DeleteItem();
                Master_Master_Button_GoHistory_Click();
            }
        }
        #endregion

        #region Master_Master_Button_Save_Click
        /// <summary>
        /// 保存数据
        /// </summary>
        void Master_Master_Button_Save_Click()
        {
            #region 判断资产编号是否重复
            if (Equ_DeskDP.CodeIsTow(txtCode.Text.Trim(), long.Parse(this.Master.MainID.Trim() == "" ? "0" : this.Master.MainID.Trim())))
            {
                PageTool.MsgBox(Page, "此资产编号已经存在,请重新输入！");
                this.Master.IsSaveSuccess = false;
                return;
            }
            #endregion

            if (Request["Page"] != "IssueBase")
            {
                if (string.IsNullOrEmpty(this.Master.MainID.Trim()))
                {
                    Equ_DeskDP ee = new Equ_DeskDP();
                    InitObject(ee);
                    ee.Deleted = (int)eRecord_Status.eNormal;
                    ee.RegUserID = long.Parse(Session["UserID"].ToString());
                    ee.RegUserName = Session["PersonName"].ToString();
                    ee.RegTime = DateTime.Now;
                    // ee.partBranchId = DeptBranch.DeptID;
                    // ee.partBranchName = DeptBranch.DeptName;
                    ee.RegDeptID = long.Parse(Session["UserDeptID"].ToString());
                    ee.RegDeptName = Session["UserDeptName"].ToString();
                    ee.AttachXml = ctrattachment1.AttachXML;
                    List<EQU_deploy> list = DymSchemeCtrList1.contorRtnValue;
                    ee.InsertRecorded(ee, list);
                    this.Master.MainID = ee.ID.ToString();
                }
                else
                {
                    Equ_DeskDP ee = new Equ_DeskDP();
                    ee = ee.GetReCorded(long.Parse(this.Master.MainID.Trim()));//获取数据
                    InitObject(ee);//获取页面数据
                    ee.AttachXml = ctrattachment1.AttachXML;

                    List<EQU_deploy> list = DymSchemeCtrList1.contorRtnValue;//获取扩展项信息
                    if (this.IsChange == true && this.ChangeFlowID != 0)//是否是变更传过来
                    {
                        //保存到临时表
                        ee.SaveToChangeTemp(ee, this.ChangeFlowID, long.Parse(Session["UserID"].ToString()), list);
                    }
                    else
                    {
                        //保存数据
                        ee.UpdateRecorded(ee, long.Parse(Session["UserID"].ToString()), list);
                    }
                    this.Master.MainID = ee.ID.ToString();
                }
            }
            else if (Request["Page"] == "IssueBase")
            {
                Equ_DeskDP ee = new Equ_DeskDP();
                InitObject(ee);
                ee.Deleted = (int)eRecord_Status.eNormal;
                ee.RegUserID = long.Parse(Session["UserID"].ToString());
                ee.RegUserName = Session["PersonName"].ToString();
                ee.RegTime = DateTime.Now;
                ee.RegDeptID = long.Parse(Session["UserDeptID"].ToString());
                ee.RegDeptName = Session["UserDeptName"].ToString();
                ee.AttachXml = ctrattachment1.AttachXML;
                List<EQU_deploy> list = DymSchemeCtrList1.contorRtnValue;
                ee.InsertRecorded(ee, list);
                this.Master.MainID = ee.ID.ToString();

                //将新增的资产信息传回事件单登记界面
                string sWhere = " And id = " + ee.ID.ToString();

                DataTable dt = ee.GetDataTable(sWhere, "");
                Json json = new Json(dt);

                string jsonstr = "{record:" + json.ToJson() + "}";

                StringBuilder sbText = new StringBuilder();
                sbText.Append("<script>");
                sbText.Append("var arr = " + jsonstr + ";");
                if (Request["TypeFrm"] != null)
                {
                    string requestType = Request.QueryString["TypeFrm"].ToString();
                    if (requestType == "CST_Issue_Service")
                    {

                        sbText.Append("if (arr != null)");
                        sbText.Append("{");
                        sbText.Append(" var json = arr;");
                        sbText.Append(" var record = json.record;");

                        sbText.Append("for (var i = 0; i < record.length; i++)");
                        sbText.Append("{");
                        sbText.Append("window.opener.document.getElementById('" + Opener_ClientId + "txtEqu').value = record[i].name;");   //设备名称
                        sbText.Append("window.opener.document.getElementById('" + Opener_ClientId + "hidEquName').value = record[i].name;");   //设备名称
                        sbText.Append("window.opener.document.getElementById('" + Opener_ClientId + "hidEqu').value = record[i].id;");  //设备ID
                        sbText.Append(" window.opener.document.getElementById('" + Opener_ClientId + "txtListName').value = record[i].listname;");   //资产目录名称
                        sbText.Append("window.opener.document.getElementById('" + Opener_ClientId + "hidListName').value = record[i].listname; ");  //资产目录名称
                        sbText.Append("window.opener.document.getElementById('" + Opener_ClientId + "hidListID').value = record[i].listid;");  //资产目录ID
                        sbText.Append(" }");
                        sbText.Append("}");
                    }
                    else if (requestType == "DemandBase")
                    {
                        sbText.Append("if (arr != null)");
                        sbText.Append("{");
                        sbText.Append(" var json = arr;");
                        sbText.Append(" var record = json.record;");

                        sbText.Append("for (var i = 0; i < record.length; i++)");
                        sbText.Append("{");
                        sbText.Append("window.opener.document.getElementById('" + Opener_ClientId + "txtEqu').value = record[i].name;");   //资产名称
                        sbText.Append("window.opener.document.getElementById('" + Opener_ClientId + "hidEquName').value = record[i].name;");   //资产名称
                        sbText.Append("window.opener.document.getElementById('" + Opener_ClientId + "hidEqu').value = record[i].id;");  //资产ID                        
                        sbText.Append("window.opener.document.getElementById('" + Opener_ClientId + "hidListName').value = record[i].listname; ");  //资产目录名称
                        sbText.Append("window.opener.document.getElementById('" + Opener_ClientId + "hidListID').value = record[i].listid;");  //资产目录ID
                        sbText.Append(" }");
                        sbText.Append("}");
                    }

                }
                else
                {
                    //=========zxl===
                    sbText.Append("if(arr != null){var record=arr.record;" +
                        "for(var i=0; i < record.length; i++){  window.opener.document.getElementById('ctl00_ContentPlaceHolder1_txtEqu').value=record[i].name;" +
                        "window.opener.document.getElementById('ctl00_ContentPlaceHolder1_hidEquName').value=record[i].name;" +
                        "window.opener.document.getElementById('ctl00_ContentPlaceHolder1_hidEqu').value=record[i].id;" +
                        "window.opener.document.getElementById('ctl00_ContentPlaceHolder1_txtListName').value=record[i].listname;" +
                    "window.opener.document.getElementById('ctl00_ContentPlaceHolder1_hidListName').value=record[i].listname;" +
                    "window.opener.document.getElementById('ctl00_ContentPlaceHolder1_hidListID').value=record[i].listid;" +
                "}" +

                "}");
                }

                // 关闭窗口
                sbText.Append("top.close();");
                sbText.Append("</script>");
                // 向客户端发送
                Page.RegisterStartupScript(DateTime.Now.ToString(), sbText.ToString());
                Response.Write(sbText.ToString());
            }

            /*     
             * Date: 2013-07-05 09:19
             * summary: 若是从变更单 > 资产信息 > 变更配置项 过来的, 则保存后自动调转
             * 回该变更单.
             * 
             * 目的, 避免其在保存后又能点击删除按钮.
             * modified: sunshaozong@gmail.com     
             */

            String strFromChangeBase = Request.QueryString["from_change_base"];
            if (strFromChangeBase != null)
            {
                if (strFromChangeBase.Trim().ToLower().Equals("yes"))
                {
                    Master_Master_Button_GoHistory_Click();
                }
            }

            if (IsChange) //从变更单过来的
            {
                PageTool.AddJavaScript(this, "window.close();");
                return;
            }

        }
        #endregion

        #region Master_Master_Button_GoHistory_Click
        /// <summary>
        /// 
        /// </summary>
        void Master_Master_Button_GoHistory_Click()
        {
            if (Request["Page"] != "IssueBase")
            {
                if (IsChange || IsRel || IsNewWin || (Request["IsTanChu"] != null && Request["IsTanChu"] == "true")) //关闭窗口的情况
                {
                    //=========zxl==
                    if (TypeFrm == "frm_ChangeBase")
                    {
                        string FlowModelIDChangeBase = Request.QueryString["FlowModelIDChangeBase"].ToString();


                        String url = "frm_ChangeBase.aspx?FlowModelID=" + FlowModelIDChangeBase;

                        /*     
                         * Date: 2012-8-6 14:40
                         * summary: 在跳转时, 为 url 追加消息ID的参数
                         * modified: sunshaozong@gmail.com     
                         */
                        String strMsgId = Request.QueryString["MessageID"];
                        if (!String.IsNullOrEmpty(strMsgId))
                            url = url + String.Format("&MessageID={0}", strMsgId);
                        else url = url + "&MessageID=0";


                        /*     
                         * Date: 2013-07-04 09:55
                         * summary: 检查URL中是否包含 from_change_base 参数且其值为 yes。
                         * 若是则在跳转时, 为 url 追加from_equ_deskedit的参数。
                         * 
                         * 参数说明,
                         * from_change_base: 表示是从 变更单中 > 资产信息 > 变更配置项 按钮跳转的。
                         * from_change_base 合法值：yes
                         * 
                         * from_equ_deskedit: 表示是从 frmEqu_DeskEdit.aspx.cs 文件中的 Master_Master_Button_GoHistory_Click 方法跳转过去的。
                         * from_equ_deskedit 合法值：yes
                         * 
                         * 目标: 修复在变更单 > 资产信息 中新增资产, 然后点击变更配置项按钮跳转到资产资料编辑页面，再点返回按钮回去时，原先所选的资产不见
                         * 了的问题。
                         * 
                         * modified: sunshaozong@gmail.com     
                         */
                        String strFromChangeBase = Request.QueryString["from_change_base"];
                        if (strFromChangeBase != null
                            && strFromChangeBase.Trim().ToLower().Equals("yes"))
                        {
                            url = url + "&from_equ_deskedit=yes";
                        }

                        Response.Redirect(url);

                    }
                    else
                    {
                        PageTool.AddJavaScript(this, "window.close();");
                    }
                    return;
                }

                string strFlowID = "0";
                if (Request["FlowID"] != null)
                {
                    strFlowID = Request["FlowID"].ToString().Trim();
                }
                if (IsSelect)
                {
                    Response.Redirect("frmEqu_DeskMain.aspx?subjectid=" + CatalogID + "&IsSelect=1" + "&FlowID=" + strFlowID);
                }
                else if (IsSameSchemaItem)
                {
                    //从查看相同配置里过来                   
                    Response.Redirect(Session["SameSchemaDetailReturnUrl"].ToString());
                }
                else
                {
                    Response.Redirect("frmEqu_DeskMain.aspx?subjectid=" + CatalogID);
                }

            }
            else if (Request["Page"] == "IssueBase")
            {
                StringBuilder sb = new StringBuilder();
                sb.Append("<script>top.close();</script>");
                //向客户端发送
                Page.RegisterStartupScript(DateTime.Now.ToString(), sb.ToString());
                Response.Write(sb.ToString());
            }
        }
        #endregion

        #region 事件单过来

        public string IsuueBase()
        {
            if (Request["Page"] != "IssueBase")
            {
                return "";
            }
            else
            {
                return "IssueBase";
            }
        }

        #endregion


        /// <summary>
        /// 获取根目录的部门
        /// </summary>
        /// <param name="deptID">当前节点的部门ID</param>
        /// <returns></returns>
        public string getPrentDeptID()
        {
            string depid = DeptDP.GetDeptFullID(StringTool.String2Long(DeptBranch.DeptID.ToString()));
            if (depid != "" && depid.Length > 5)
            {
                depid = depid;
            }
            return depid;
        }

        #region Page_Load
        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void Page_Load(object sender, System.EventArgs e)
        {
            SetParentButtonEvent();

            CtrEquCataDropList1.mySelectedIndexChanged += new EventHandler(CtrEquCataDropList1_mySelectedIndexChanged);
            DeptPickerbank2.bankTexChange += new EventHandler(BranchTexChange);

            if (!IsPostBack)
            {
                InitddlMastCust(); //初始化服务单位

                string strThisMsg = "";
                strThisMsg = txtCode.ClientID + ">" + "资产编号";
                this.Page.ClientScript.RegisterStartupScript(this.GetType(), txtCode.ClientID, "<script>if(typeof(sarrvalidlist) == 'undefined'){ var sarrvalidlist = new Array(1);}  sarrvalidlist[0] = sarrvalidlist[0] + '|' + '" + strThisMsg + "';</script>");

                //保修期初始化为空
                dtServiceBeginTime.AllowNull = true;
                dtServiceBeginTime.dateTimeString = "";
                dtServiceEndTime.AllowNull = true;
                dtServiceEndTime.dateTimeString = "";

                if (Request["subjectid"] != null)
                {
                    CtrEquCataDropList1.CatelogID = decimal.Parse(Request["subjectid"].ToString());
                    //新增时的初值
                    lngCatalogID = long.Parse(Request["subjectid"].ToString());
                }
                if (Request["Page"] == null || Request["Page"] != "IssueBase")
                    CtrEquCataDropList1.ContralState = eOA_FlowControlState.eReadOnly;

                //资产所属用户
                decimal customID = 0;
                LoadData(ref customID);

                //从事件单过来的
                if (Request["Page"] == "IssueBase")
                {

                }

                //设置显示
                PageDeal.SetLanguage(this.Controls[0]);

                //页面初始化时，根据用户权限判断是否全设为只读；1、判断此资产所属类别是否在用户权限范围内的资产类别列表中；2、判断用户是否在资产的维护部门中
                if (this.Master.MainID != null && this.Master.MainID != "")
                {
                    //从查询列表过来的；不是新建
                    string sEquList = Session["EquLimitList"].ToString() + ",";

                    bool isPermissions = false;                 

                    EA_ExtendRightsDP ee = new EA_ExtendRightsDP();
                    string sWhere = string.Empty;
                    sWhere += " And OperateID=" + lngCatalogID.ToString();
                    sWhere += " Order by ObjectType";
                    DataTable dt = ee.GetRightInfo(sWhere);

                    foreach (DataRow dr in dt.Rows)
                    {
                        string objectID = dr["ObjectID"].ToString();
                        switch (dr["ObjectType"].ToString())
                        {
                            case "人员":
                                if (objectID == Session["UserID"].ToString())
                                {
                                    isPermissions = true;
                                }
                                break;
                            case "部门":
                                if (objectID == Session["UserDeptID"].ToString())
                                {
                                    isPermissions = true;
                                }
                                break;
                            default:
                                isPermissions = UserDP.IsBelongActor(StringTool.String2Long(Session["UserID"].ToString()), StringTool.String2Long(objectID));
                                break;
                        }
                        if (isPermissions)
                        {
                            break;
                        }
                    }


                    #region 查询 用户对应部门和对应子部门ID 陈志文
                    string parentids = GetParentIDs();
                    #endregion


                    if (isPermissions 
                        || sEquList.Contains(lngCatalogID.ToString() + ",") 
                        || DeptDP.getExistUserByDept(Session["UserID"].ToString(), parentids, DeptBranch.DeptID.ToString()))
                    {
                        //满足以上1、2中任意一条                        
                    }
                    else
                    {
                        //检查资产所属客户对应的用户是否当前用户,upt by wxh
                        Br_ECustomerDP2 cusDal = new Br_ECustomerDP2();
                        var cus= cusDal.GetReCorded(long.Parse(customID.ToString()));
                        if (cus.UserID.ToString() != Session["UserID"].ToString())
                        {
                            //否则
                            SetFormReadOnly();

                            this.Master.ShowNewButton(false);
                            this.Master.ShowDeleteButton(false);
                            this.Master.ShowSaveButton(false);
                        }
                    }
                }
            }
            else
            {
                lngCatalogID = long.Parse(ViewState["Equ_DeskEdit_CatalogID"].ToString());
                //历史版本的schema
                if (this.ChartVersion != -1)
                {
                    sHistorySchema = ViewState["frmDeskEditsHistorySchema"].ToString();
                }
                if (this.hidProvideName.Value.Trim() != this.txtProvideName.Text.Trim())
                {
                    this.txtProvideName.Text = this.hidProvideName.Value.Trim();
                }
            }


            #region 解决页面刷新后文本框值丢失的问题
            if (hidCateListName.Value.Trim() != "")
                txtCateList.Text = hidCateListName.Value.Trim();
            #endregion

            //判断是否是变更过来 同时 这笔变更单是否完成
            bool isConvert = false;
            if (this.ChangeFlowID > 0)
            {
                string strSQL = "select * from es_flow  where FlowID=" + this.ChangeFlowID + " and status=30";
                DataTable dtChange = CommonDP.ExcuteSqlTable(strSQL);
                if (dtChange != null && dtChange.Rows.Count > 0)
                    isConvert = true;
            }


            if (IsChange == true && IsChangeEdit == false && isConvert == true)
            {
                #region 从变更单过来查看资产详情 需要比较历史版本  yxq 2011-10-12
                //DymSchemeCtr1.ControlXmlValue = ee.ConfigureValue;

                if (!string.IsNullOrEmpty(this.Master.MainID.Trim()))
                {
                    long lngID = long.Parse(this.Master.MainID.Trim());
                    Equ_DeskDP ee = new Equ_DeskDP();

                    if (this.ChartVersion == -1)
                    {
                        //当前版本情况下
                        if (this.IsChange == true && this.ChangeFlowID != 0)
                        {
                            ee = ee.GetReCordedForChange(lngID, this.ChangeFlowID);

                        }
                        else
                        {
                            ee = ee.GetReCorded(lngID);
                        }
                    }
                    else
                    {
                        //历史版本情况
                        ee = ee.GetReCordedForVersion(lngID, this.ChartVersion);
                    }

                    if ((ee.Version + 1) == -1)
                    {
                        DymSchemeCtrList1.ReadOnly = true;
                        DymSchemeCtrList1.EquID = long.Parse(ee.ID.ToString());
                        DymSchemeCtrList1.EquCategoryID = long.Parse(ee.CatalogID.ToString());
                    }
                    else
                    {

                        DymSchemeCtrList1.ReadOnly = true;
                        DymSchemeCtrList1.EquID = long.Parse(ee.ID.ToString());
                        DymSchemeCtrList1.isVersion = true;
                        DymSchemeCtrList1.Version = long.Parse((ee.Version + 1).ToString());
                        DymSchemeCtrList1.EquCategoryID = long.Parse(ee.CatalogID.ToString());
                    }
                }
                #endregion
            }
            else
            {
                if (this.Master.MainID == "")
                {
                    DymSchemeCtrList1.EquID = 0;
                }
                else
                {
                    DymSchemeCtrList1.EquID = long.Parse(this.Master.MainID);

                    if (ChangeFlowID != 0)
                    {
                        //变更临时表记录
                        DymSchemeCtrList1.isChange = true;
                        DymSchemeCtrList1.FlowID = this.ChangeFlowID;
                    }
                    else if (ChartVersion != -1)
                    {
                        DymSchemeCtrList1.Version = ChartVersion;
                    }

                }
                DymSchemeCtrList1.EquCategoryID = lngCatalogID;
            }

        }

        #region 获取当前部门，对应的所有子部门和本部门的ID
        public string GetParentIDs()
        {
            string parentid = DeptDP.GetParentID(Session["UserID"].ToString());
            string deptid = DeptBranch.DeptID.ToString();
            string parentids = "";
            int parentindex = parentid.IndexOf(deptid);
            if (parentindex >= 0)
            {
                if (parentid.Length > 5)
                {
                    int i = parentindex;

                    while (i < parentid.Length)
                    {
                        parentids = parentids + parentid.Substring(i, 5) + ",";
                        i = i + 6;
                    }
                }
                else
                {
                    parentids = deptid;
                }

                if (parentids.Substring(parentids.Length - 1, 1) == ",")
                {
                    parentids = parentids.Substring(0, parentids.Length - 1);
                }
            }
            return parentids;
        }
        #endregion

        #region 初始化服务单位下拉列表的值

        /// <summary>
        /// 
        /// </summary>
        protected void InitddlMastCust()
        {
            Br_MastCustomerDP ee = new Br_MastCustomerDP();
            String sWhere = String.Empty;
            String sOrder = String.Empty;
            DataTable dt = ee.GetDataTable(sWhere, sOrder);
            ddlMastCust.DataSource = dt;
            ddlMastCust.DataTextField = "ShortName";
            ddlMastCust.DataValueField = "ID";
            ddlMastCust.DataBind();
            ddlMastCust.Items.Insert(0, new ListItem("", "0"));
        }
        #endregion

        #region 维护机构变化时，改变维护部门的父ID值
        /// <summary>
        /// 维护机构变化时，改变维护部门的父ID值
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        void BranchTexChange(object sender, EventArgs e)
        {
            //zxl==
            //  DeptBranch.DeptID = 0;
            //  DeptBranch.DeptName = "";
            DeptBranch.ParentID = DeptPickerbank2.DeptID.ToString();
            lblCateList.Text = hidCateListName.Value;
            txtCateList.Text = hidCateListName.Value;
        }
        #endregion

        void CtrEquCataDropList1_mySelectedIndexChanged(object sender, EventArgs e)
        {
            if (DymSchemeCtrList1.EquCategoryID != CtrEquCataDropList1.CatelogID)
            {
                long lngEquCateID = (long)CtrEquCataDropList1.CatelogID;
                ViewState["Equ_DeskEdit_CatalogID"] = lngEquCateID;   //保存 最新的分类ID

                DymSchemeCtrList1.EquCategoryID = lngEquCateID;

                //当资产类别改变时，还要改变lngCatalogID的值；便于选择资产目录时，传递对应的类别ID
                lngCatalogID = lngEquCateID;

                txtCateList.Text = string.Empty;
                lblCateList.Text = string.Empty;
                hidCateListName.Value = string.Empty;
                hiCateListID.Value = "-1";
            }
        }
        #endregion

        #region LoadData
        /// <summary>
        /// 
        /// </summary>
        private void LoadData(ref decimal customID)
        {
            if (!string.IsNullOrEmpty(this.Master.MainID.Trim()))
            {
                long lngID = long.Parse(this.Master.MainID.Trim());
                Equ_DeskDP ee = new Equ_DeskDP();

                if (this.ChartVersion == -1)
                {
                    //当前版本情况下
                    if (this.IsChange == true && this.ChangeFlowID != 0)
                    {
                        ee = ee.GetReCordedForChange(lngID, this.ChangeFlowID);

                    }
                    else
                    {
                        ee = ee.GetReCorded(lngID);
                    }
                }
                else
                {
                    //历史版本情况
                    ee = ee.GetReCordedForVersion(lngID, this.ChartVersion);

                    sHistorySchema = ee.HistorySchema;
                    ViewState["frmDeskEditsHistorySchema"] = sHistorySchema;
                }

                //返回所属用户编号，upt by wxh
                customID = ee.Costom;

                #region 给附件控件赋值
                ctrattachment1.AttachmentType = eOA_AttachmentType.eZC;
                ctrattachment1.FlowID = (long)ee.ID;
                #endregion


                CtrFTName.Value = ee.Name.ToString();
                txtCode.Text = ee.Code.ToString();
                txtPositions.Text = ee.Positions.ToString();
                txtSerialNumber.Text = ee.SerialNumber.ToString();
                hidProvideID.Value = ee.Provide.ToString();
                hidProvideName.Value = ee.ProvideName.ToString();
                txtProvideName.Text = ee.ProvideName.ToString();
                CtrEquCataDropList1.CatelogID = ee.CatalogID;
                txtConfigureInfo.Text = ee.ConfigureInfo.ToString();
                CustomCtr1.CustomID = ee.Costom;
                CustomCtr1.CustomName = ee.CostomName;
                dtServiceBeginTime.AllowNull = false;
                dtServiceBeginTime.dateTime = ee.ServiceBeginTime;
                dtServiceEndTime.AllowNull = false;
                dtServiceEndTime.dateTime = ee.ServiceEndTime;
                txtItemCode.Text = ee.ItemCode;
                txtBreed.Text = ee.Breed.Trim();
                txtModel.Text = ee.Model.Trim();

                lblCode.Text = ee.Code.ToString();
                lblPositions.Text = ee.Positions.ToString();
                lblSerialNumber.Text = ee.SerialNumber.ToString();
                lblProvideName.Text = ee.ProvideName.ToString();
                lblConfigureInfo.Text = ee.ConfigureInfo.ToString();

                ctrFlowCataEquStatus.CatelogID = (long)ee.EquStatusID;
                ctrFlowCataEquStatus.CatelogValue = ee.EquStatusName;

                //变更时的的历史表展示逻辑控制
                if (ee.Version == 1)
                {
                    //当正在变更时
                    DymSchemeCtrList1.isChange = true;
                }
                else
                {
                    //当变更完成时，默认变更完成
                    DymSchemeCtrList1.isChange = false;
                }

                lblItemCode.Text = ee.ItemCode.ToString();
                lblCustomCtr1.Text = ee.CostomName.ToString();
                hidCustID.Value = ee.Costom.ToString();
                lblServiceBeginTime.Text = ee.ServiceBeginTime.ToString("yyyy-MM-dd");
                lblServiceEndTime.Text = ee.ServiceEndTime.ToString("yyyy-MM-dd");

                DeptPickerbank2.DeptID = long.Parse(ee.partBankId.ToString());
                DeptPickerbank2.DeptName = ee.partBankName;
                DeptBranch.DeptID = long.Parse(ee.partBranchId.ToString());
                DeptBranch.DeptName = ee.partBranchName;
                DeptBranch.ParentID = DeptPickerbank2.DeptID.ToString();

                lblBreed.Text = ee.Breed.Trim();
                lblModel.Text = ee.Model.Trim();

                hiCateListID.Value = ee.ListID.ToString();
                hidCateListName.Value = ee.ListName;
                txtCateList.Text = ee.ListName;
                lblCateList.Text = ee.ListName;

                sSchemaValue = ee.ConfigureValue;

                lngCatalogID = (long)ee.CatalogID;

                if (IsChange == true && IsChangeEdit == false)
                {
                    //变更单过来并且非编辑状态显示版本比较
                    #region
                    Equ_DeskDP ePre = new Equ_DeskDP();

                    if (ePre.HasChangeTempToHistory(lngID, (long)ee.Version) == true)
                    {
                        ePre = ePre.GetReCordedForVersion(lngID, (long)ee.Version);
                    }
                    else
                    {
                        ePre = ePre.GetReCorded(lngID);
                    }


                    if (lblCateList.Text != ePre.ListName.ToString())//资产目录
                        lblCateList.ForeColor = Color.Red;

                    if (ee.Name.ToString() != ePre.Name.ToString())
                        CtrFTName.foreColor = Color.Red;
                    //lblName.ForeColor = Color.Red;

                    if (ee.Code.ToString() != ePre.Code.ToString())
                        lblCode.ForeColor = Color.Red;

                    if (ee.Mastcustid.ToString() != ePre.Mastcustid.ToString())
                        lblMastCust.ForeColor = Color.Red;

                    if (ee.Positions.ToString() != ePre.Positions.ToString())
                        lblPositions.ForeColor = Color.Red;

                    if (ee.SerialNumber.ToString() != ePre.SerialNumber.ToString())
                        lblSerialNumber.ForeColor = Color.Red;

                    if (ee.ProvideName.ToString() != ePre.ProvideName.ToString())
                        lblProvideName.ForeColor = Color.Red;

                    if (ee.ItemCode.ToString() != ePre.ItemCode)
                        lblItemCode.ForeColor = Color.Red;

                    if (ee.CostomName != ePre.CostomName)
                        lblCustomCtr1.ForeColor = Color.Red;

                    if (ee.Breed != ePre.Breed)
                        lblBreed.ForeColor = Color.Red;

                    if (ee.Model != ePre.Model)
                        lblModel.ForeColor = Color.Red;

                    if (ee.ServiceBeginTime != ePre.ServiceBeginTime)
                        lblServiceBeginTime.ForeColor = Color.Red;

                    if (ee.ServiceEndTime != ePre.ServiceEndTime)
                        lblServiceEndTime.ForeColor = Color.Red;

                    if (ee.CatalogID != ePre.CatalogID)
                        CtrEquCataDropList1.ForColor = Color.Red; ;

                    if (ee.EquStatusID != ePre.EquStatusID)
                        ctrFlowCataEquStatus.ForColor = Color.Red;

                    if (ee.partBankId != ePre.partBankId)
                    {
                        DeptPickerbank2.ForColor = Color.Red;
                    }
                    if (ee.partBranchId != ePre.partBranchId)
                        DeptBranch.ForColor = Color.Red;

                    #endregion
                }

                //服务单位
                ddlMastCust.SelectedIndex = ddlMastCust.Items.IndexOf(ddlMastCust.Items.FindByValue(ee.Mastcustid.ToString()));
                lblMastCust.Text = ddlMastCust.SelectedItem.Text;

            }
            else
            {
                //新增状况下，为控件设置第一次加载时的新增状态

                //新增状态下 有效期自动加一年
                dtServiceBeginTime.dateTime = DateTime.Now;
                dtServiceEndTime.dateTime = DateTime.Now.AddYears(1);

                #region   测试控件值 配置控件
                //DymSchemeCtr1.SetAddEquTrue();
                #endregion
            }


            #region   测试控件值 配置控件
            //Session["Equ_DeskEdit_Schema"] = sSchemaValue;
            #endregion
            ViewState["Equ_DeskEdit_CatalogID"] = lngCatalogID;


        }
        #endregion

        #region InitObject
        /// <summary>
        /// 
        /// </summary>
        /// <param name="ee"></param>
        private void InitObject(Equ_DeskDP ee)
        {
            ee.Name = CtrFTName.Value.Trim();
            ee.Code = txtCode.Text.Trim();
            ee.Positions = txtPositions.Text.Trim();
            ee.SerialNumber = txtSerialNumber.Text.Trim();
            if (string.IsNullOrEmpty(hidProvideID.Value.Trim()))
                ee.Provide = 0;
            else
                ee.Provide = decimal.Parse(hidProvideID.Value.Trim());
            ee.ProvideName = hidProvideName.Value.Trim();
            txtProvideName.Text = hidProvideName.Value.Trim();
            ee.CatalogID = decimal.Parse(CatalogID);
            ee.CatalogName = CatalogName;
            ee.FullID = FullID;
            ee.ConfigureInfo = txtConfigureInfo.Text.Trim();
            ee.Costom = CustomCtr1.CustomID;
            ee.CostomName = CustomCtr1.CustomName.Trim();
            ee.ServiceBeginTime = dtServiceBeginTime.dateTime;
            ee.ServiceEndTime = dtServiceEndTime.dateTime;
            ee.ItemCode = txtItemCode.Text.Trim();

            ee.Breed = txtBreed.Text.Trim();
            ee.Model = txtModel.Text.Trim();


            ee.ConfigureValue = ""; // DymSchemeCtr1.ControlXmlValue;
            ee.EquStatusID = ctrFlowCataEquStatus.CatelogID;
            ee.EquStatusName = ctrFlowCataEquStatus.CatelogValue;

            ee.ListID = decimal.Parse(hiCateListID.Value == "" ? "0" : hiCateListID.Value);
            ee.ListName = hidCateListName.Value.ToString().Trim();


            ee.partBankId = DeptPickerbank2.DeptID;
            ee.partBankName = DeptPickerbank2.DeptName;
            ee.partBranchId = DeptBranch.DeptID;
            ee.partBranchName = DeptBranch.DeptName;
            if (ddlMastCust.SelectedItem != null)
                ee.Mastcustid = decimal.Parse(ddlMastCust.SelectedItem.Value);
            else
                ee.Mastcustid = 0;

            txtCateList.Text = hidCateListName.Value.Trim();
            lblCateList.Text = hidCateListName.Value.Trim();

            ViewState["Equ_DeskEdit_CatalogID"] = ee.CatalogID;
            Session["Equ_DeskEdit_Schema"] = ee.ConfigureValue;

        }
        #endregion

        #region  SetFormReadOnly
        /// <summary>
        /// 
        /// </summary>
        private void SetFormReadOnly()
        {
            CtrFTName.ContralState = eOA_FlowControlState.eReadOnly;
            txtCode.Visible = false;
            txtPositions.Visible = false;
            txtSerialNumber.Visible = false;
            txtProvideName.Visible = false;
            cmdPop.Visible = false;
            CtrEquCataDropList1.ContralState = eOA_FlowControlState.eReadOnly;
            txtConfigureInfo.Visible = false;
            CustomCtr1.Visible = false;
            CustomCtr1.Visible = false;
            dtServiceBeginTime.Visible = false;
            dtServiceEndTime.Visible = false;
            txtItemCode.Visible = false;
            txtModel.Visible = false;
            txtBreed.Visible = false;

            lblCode.Visible = true;
            lblPositions.Visible = true;
            lblSerialNumber.Visible = true;
            lblProvideName.Visible = true;
            lblConfigureInfo.Visible = true;
            lblItemCode.Visible = true;
            lblCustomCtr1.Visible = true;
            lblServiceBeginTime.Visible = true;
            lblServiceEndTime.Visible = true;
            lblModel.Visible = true;
            lblBreed.Visible = true;

            Button1.Visible = false;

            DymSchemeCtrList1.ReadOnly = true;
            ctrFlowCataEquStatus.ContralState = eOA_FlowControlState.eReadOnly;
            DeptPickerbank2.ReadOnly = true;
            DeptBranch.ReadOnly = true;

            txtCateList.Visible = false;
            lblCateList.Visible = true;

            MustCate.Visible = false;
            MustCode.Visible = false;

            ctrattachment1.ReadOnly = true;
            ddlMastCust.Visible = false;
            lblMastCust.Visible = true;
        }
        #endregion
    }
}