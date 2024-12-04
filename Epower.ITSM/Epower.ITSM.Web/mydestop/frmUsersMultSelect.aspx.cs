using System;
using System.Collections;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Xml.Linq;
using System.Text;
using Epower.DevBase.Organization.SqlDAL;
using Epower.DevBase.Organization.Base;
using Epower.DevBase.Organization;
using Epower.DevBase.BaseTools;

namespace Epower.ITSM.Web.mydestop
{
    public partial class frmUsersMultSelect : BasePage
    {
        public string RootImg
        {
            get
            {
                if (ViewState["RootImg"] != null)
                    return ViewState["RootImg"].ToString();
                else
                    return "..\\Images\\root.gif";
            }
            set
            {
                ViewState["RootImg"] = value;
            }
        }

        public string NodeImg
        {
            get
            {
                if (ViewState["NodeImg"] != null)
                    return ViewState["NodeImg"].ToString();
                else
                    return "..\\Images\\FlowDesign\\333.ico";
            }
            set
            {
                ViewState["NodeImg"] = value;
            }
        }

        //protected System.Web.UI.WebControls.Button cmdLoad;
        long deptId;
        string strUserID;
        bool bIncludeChildTree = false;

        protected string strZHHiden = "";
        protected string strZHShow = "display:none;";
        protected void Page_Load(object sender, EventArgs e)
        {
            SetParentButtonEvent();
            ControlPageUserInfo.On_PostBack += new EventHandler(ControlPageUserInfo_On_PostBack);
            ControlPageUserInfo.DataGridToControl = dgUserInfo;
            strUserID = Session["UserID"].ToString();
            ctrFCDServiceType.mySelectedIndexChanged += new EventHandler(ctrFCDServiceType_mySelectedIndexChanged);
            CtrDeptTree1.SelectedChangeEvent += new Epower.ITSM.Web.DeptControls.CtrDeptTree.SelectedChangeHandler(CtrDeptTree1_SelectedChangeEvent);


            if (Request.QueryString["ExtAll"] != null)
            {
                if (Request.QueryString["ExtAll"] == "true")
                {
                    bIncludeChildTree = true;
                }
            }
            //========zxl==
            if (!IsPostBack)
            {
                deptId = Convert.ToInt32(Request.Params.Get("DeptID"));
                // 记录当前部门
                Session["OldDeptID"] = deptId;
                hidDeptID.Value = deptId.ToString();
                hidQueryDeptID.Value = hidDeptID.Value;
                txtDeptName.Text = DeptDP.GetDeptName(deptId);//获取部门名


                LoadData();
                BindData();


            }
            //===zxl==

            if (Page.IsPostBack == false)
            {
                //InitTreeView();
                Session["deptvalue"] = "1";
                LoadData_new();

            }

        }

        void CtrDeptTree1_SelectedChangeEvent(object sender, TreeNode node)
        {
            if (node != null && node.Value != null)
            {
                hidQueryDeptID.Value = node.Value;

                btnQuery_Click();
            }
        }

        public string Opener_ClientId
        {

            get
            {
                return (Request["Opener_ClientId"] == null) ? "" : Request["Opener_ClientId"].ToString().Replace("hidClientId_ForOpenerPage", "");
            }
        }
        //============================================================
        #region 是否限制选择范围
        /// <summary>
        /// 
        /// </summary>
        private bool mIsLimit = false;
        /// <summary>
        /// 是否限制部门范围
        /// </summary>
        public bool IsLimit
        {
            get
            {
                if (Request.QueryString["LimitCurr"] != null && Request.QueryString["LimitCurr"].ToLower() == "true")
                {
                    return true;
                }
                else
                {
                    return false;
                }
            }
        }
        #endregion

        #region SetParentButtonEvent
        /// <summary>
        /// 
        /// </summary>
        protected void SetParentButtonEvent()
        {
            this.Master.Master_Button_Query_Click += new Global_BtnClick(Master_Master_Button_Query_Click);
            this.Master.ShowQueryButton(true);
        }
        #endregion

        #region Master_Master_Button_Query_Click
        /// <summary>
        /// 
        /// </summary>
        void Master_Master_Button_Query_Click()
        {
            btnQuery_Click();
        }
        #endregion




        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        void ctrFCDServiceType_mySelectedIndexChanged(object sender, EventArgs e)
        {
            btnQuery_Click();
        }

        /// <summary>
        /// 
        /// </summary>
        private void LoadData()
        {
            string RootDeptID = "1";
            DataTable dt = Exec_Query(RootDeptID, bIncludeChildTree);
            Session["USERINFO_DATA"] = dt;
        }

        /// <summary>
        /// 
        /// </summary>
        private void BindData()
        {
            DataTable dt = (DataTable)Session["USERINFO_DATA"];
            this.dgUserInfo.DataSource = dt.DefaultView;
            this.dgUserInfo.DataBind();
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="UserID"></param>
        /// <returns></returns>
        protected string GetVisible(string UserID)
        {
            string sResult;
            if (strUserID.Equals(UserID))
                sResult = "VISIBILITY:hidden";
            else
                sResult = "VISIBILITY:visible";

            return sResult;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="Title"></param>
        /// <param name="FunctionName"></param>
        /// <param name="UserName"></param>
        /// <param name="Email"></param>
        /// <returns></returns>
        protected string GetEmailAction(string Title, string FunctionName, string UserName, string Email)
        {
            string sResult = "";

            if (Email.Trim().Length > 0)
                sResult = "<A href='#' title='点击[" + Title + "]将 " + UserName + " 加入" + Title + "列表' " +
                        "onclick=\"" + FunctionName + "('" + UserName + "','" + Email + "')\"" +
                        ">" + Title + "</A>";
            return sResult;
        }

        #region Web 窗体设计器生成的代码
        override protected void OnInit(EventArgs e)
        {
            //
            // CODEGEN: 该调用是 ASP.NET Web 窗体设计器所必需的。

            //
            InitializeComponent();
            base.OnInit(e);
        }

        /// <summary>
        /// 设计器支持所需的方法 - 不要使用代码编辑器修改

        /// 此方法的内容。

        /// </summary>
        private void InitializeComponent()
        {
            this.dgUserInfo.ItemCreated += new System.Web.UI.WebControls.DataGridItemEventHandler(this.dgUserInfo_ItemCreated);

        }
        #endregion

        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void ControlPageUserInfo_On_PostBack(object sender, EventArgs e)
        {
            //LoadData();
            BindData();
        }

        /// <summary>
        /// 
        /// </summary>
        private void btnQuery_Click()
        {
            //==zxl==
            Session["OldDeptID"] = hidQueryDeptID.Value;
            hidDeptID.Value = hidQueryDeptID.Value;
            txtDeptName.Text = DeptDP.GetDeptName(long.Parse(hidQueryDeptID.Value));//获取部门名



            bIncludeChildTree = true;
            LoadData();
            BindData();
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="sDeptRoot"></param>
        /// <param name="bIncludeChildTree"></param>
        /// <returns></returns>
        private DataTable Exec_Query(string sDeptRoot, bool bIncludeChildTree)
        {
            string[][] arrayQueryParam = new string[8][];

            for (int i = 0; i < arrayQueryParam.Length; i++)
            {
                arrayQueryParam[i] = new string[2];
            }


            //部门ID
            arrayQueryParam[0][0] = "DeptID";
            if (this.hidQueryDeptID.Value.Trim().Length > 0)
            {
                arrayQueryParam[0][1] = this.hidQueryDeptID.Value.Trim();
            }
            else
            {
                arrayQueryParam[0][1] = "";
            }

            //职位
            arrayQueryParam[1][0] = "Position";
            if (ctrFCDServiceType.CatelogID != 1014)
            {
                arrayQueryParam[1][1] = ctrFCDServiceType.CatelogValue.Trim();
            }
            else
            {
                arrayQueryParam[1][1] = "";
            }


            //学历
            arrayQueryParam[2][0] = "Education";
            arrayQueryParam[2][1] = ddlEdu.SelectedValue;


            //Email
            arrayQueryParam[3][0] = "Email";
            if (txtEmail.Text.Trim().Length > 0)
            {
                arrayQueryParam[3][1] = txtEmail.Text.Trim();
            }
            else
            {
                arrayQueryParam[3][1] = "";
            }

            //登陆账号
            arrayQueryParam[4][0] = "LoginName";
            if (txtLoginName.Text.Trim().Length > 0)
            {
                arrayQueryParam[4][1] = txtLoginName.Text.Trim();
            }
            else
            {
                arrayQueryParam[4][1] = "";
            }


            //用户姓名
            arrayQueryParam[5][0] = "Name";
            if (txtName.Text.Trim().Length > 0)
            {
                arrayQueryParam[5][1] = txtName.Text.Trim();
            }
            else
            {
                arrayQueryParam[5][1] = "";
            }

            //电话
            arrayQueryParam[6][0] = "TEL";
            if (this.txtTEL.Text.Trim().Length > 0)
            {
                arrayQueryParam[6][1] = txtTEL.Text.Trim();
            }
            else
            {
                arrayQueryParam[6][1] = "";
            }

            //根部门FullID

            arrayQueryParam[7][0] = "SortBy";
            arrayQueryParam[7][1] = this.ddlSort.SelectedValue;


            Session["arrayQueryParam"] = arrayQueryParam;

            DataTable dt = UserDP.GetUsers(arrayQueryParam, sDeptRoot, bIncludeChildTree);
            return dt;

        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void dgUserInfo_ItemCreated(object sender, System.Web.UI.WebControls.DataGridItemEventArgs e)
        {
            if (e.Item.ItemType == ListItemType.Header)
            {
                DataGrid dg = (DataGrid)sender;
                for (int i = 0; i < e.Item.Cells.Count; i++)
                {
                    // (DataView)e.Item.NamingContainer;
                    if (i > 0 && i < 6)
                    {
                        int j = i - 1;   //注意,因为前面有一个不可见的列
                        e.Item.Cells[i].Attributes.Add("onclick", "sortTable('" + dg.ClientID + "'," + j.ToString() + ",0);");
                    }
                }
            }
        }

        protected void btnSelect_Click(object sender, EventArgs e)
        {

            StringBuilder stUserIDList = new StringBuilder();
            StringBuilder stUserNameList = new StringBuilder();
            foreach (DataGridItem itm in dgUserInfo.Items)
            {
                if (itm.ItemType == ListItemType.AlternatingItem ||
                    itm.ItemType == ListItemType.Item)
                {
                    CheckBox chkdel = (CheckBox)itm.Cells[0].FindControl("chkSelect");
                    if (chkdel.Checked)
                    {
                        stUserIDList.Append(itm.Cells[1].Text + ",");
                        stUserNameList.Append(itm.Cells[2].Text + ",");
                    }
                }
            }
            StringBuilder sbText = new StringBuilder();
            sbText.Append("<script>");
            sbText.Append("var arr = new Array();");
            // 用户ID列表
            sbText.Append("arr[0] ='" + stUserIDList.ToString() + "';");
            // 用户名列表

            sbText.Append("arr[1] ='" + stUserNameList.ToString() + "';");
            sbText.Append("arr[2] ='" + "edit" + "';");
            sbText.Append("window.parent.returnValue = arr;");
            // 关闭窗口
            sbText.Append("top.close();");
            sbText.Append("</script>");
            ClientScript.RegisterClientScriptBlock(GetType(), DateTime.Now.ToString(), sbText.ToString());
            Response.Write(sbText.ToString());
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void btnAdd_Click(object sender, EventArgs e)
        {
            StringBuilder stUserIDList = new StringBuilder();
            StringBuilder stUserNameList = new StringBuilder();
            foreach (DataGridItem itm in dgUserInfo.Items)
            {
                if (itm.ItemType == ListItemType.AlternatingItem ||
                    itm.ItemType == ListItemType.Item)
                {
                    CheckBox chkdel = (CheckBox)itm.Cells[0].FindControl("chkSelect");
                    if (chkdel.Checked)
                    {
                        stUserIDList.Append(itm.Cells[1].Text + ",");
                        stUserNameList.Append(itm.Cells[2].Text + ",");
                    }
                }
            }
            StringBuilder sbText = new StringBuilder();
            sbText.Append("<script>");
            sbText.Append("var arr = new Array();");
            // 用户ID列表
            sbText.Append("arr[0] ='" + stUserIDList.ToString() + "';");
            // 用户名列表

            sbText.Append("arr[1] ='" + stUserNameList.ToString() + "';");
            sbText.Append("arr[2] ='" + "add" + "';");

            string strs = " if (arr != null) { if (arr.length > 1){ if (arr[2] == 'add'){ var suserid = arr[0].split(','); var susername = arr[1].split(','); var solduserid = window.opener.document.getElementById('" + Opener_ClientId + "hidUser').value; for (i = 0; i < suserid.length; i++){";
            strs += "if (solduserid.indexOf(suserid[i]) == -1){ window.opener.document.getElementById('" + Opener_ClientId + "txtUser').value = window.opener.document.getElementById('" + Opener_ClientId + "txtUser').value + susername[i] + ',';";
            strs += "window.opener.document.getElementById('" + Opener_ClientId + "hidUserName').value = window.opener.document.getElementById('" + Opener_ClientId + "hidUserName').value + susername[i] + ',';";
            strs += "window.opener.document.getElementById('" + Opener_ClientId + "hidUser').value = window.opener.document.getElementById('" + Opener_ClientId + "hidUser').value + suserid[i] + ',';";
            strs += "  }";
            strs += "  }";
            strs += "  } ";
            strs += " else ";
            strs += " { ";
            strs += " window.opener.document.getElementById('" + Opener_ClientId + "txtUser').value = arr[1];";
            strs += " window.opener.document.getElementById('" + Opener_ClientId + "hidUserName').value = arr[1];";
            strs += " window.opener.document.getElementById('" + Opener_ClientId + "hidUser').value = arr[0];";
            strs += "  }";
            strs += " }";
            strs += " else";
            strs += "  {";
            strs += " window.opener.document.getElementById('" + Opener_ClientId + "txtUser').value = '';";
            strs += " window.opener.document.getElementById('" + Opener_ClientId + "hidUserName').value = '';";
            strs += "      window.opener.document.getElementById('" + Opener_ClientId + "hidUser').value = 0;";
            strs += "  }";
            strs += "  }";
            strs += " else";
            strs += "   {";
            strs += " window.opener.document.getElementById('" + Opener_ClientId + "txtUser').value = '';";

            strs += " window.opener.document.getElementById('" + Opener_ClientId + "hidUserName').value = '';";
            strs += "window.opener.document.getElementById('" + Opener_ClientId + "hidUser').value = 0;";
            strs += " }";

            sbText.Append(strs);


            // 关闭窗口
            sbText.Append("top.close();");
            sbText.Append("</script>");
            ClientScript.RegisterClientScriptBlock(GetType(), DateTime.Now.ToString(), sbText.ToString());
            Response.Write(sbText.ToString());
        }


        private string strNodeIndex = "0";
        private long lngCurrDeptID = 0;


        ///// <summary>
        ///// 设置树的高度
        ///// </summary>
        //public System.Web.UI.WebControls.Unit TreeHeight
        //{
        //    set { lngCurrDeptID = value; }
        //}

        ///// <summary>
        ///// 设置树的宽度
        ///// </summary>
        //public System.Web.UI.WebControls.Unit TreeWidth
        //{
        //    set { lngCurrDeptID = value; }
        //}

        /// <summary>
        /// 显示部门树的时候是否包含本部门及下级部门

        /// </summary>
        public bool LimitCurr
        {
            get
            {
                return ViewState["LimitCurr"] == null ? false : bool.Parse(ViewState["LimitCurr"].ToString());
            }
            set { ViewState["LimitCurr"] = value; }
        }

        /// <summary>
        /// 当前部门编号
        /// </summary>
        public long CurrDeptID
        {
            get { return ViewState["CurrDeptID"] == null ? 0 : StringTool.String2Long(ViewState["CurrDeptID"].ToString()); }
            set { ViewState["CurrDeptID"] = value; }
        }




        /// <summary>
        /// 是否设置权限
        /// </summary>
        public long IsPower
        {
            get { return ViewState["IsPower"] == null ? 0 : StringTool.String2Long(ViewState["IsPower"].ToString()); }
            set { ViewState["IsPower"] = value; }
        }

        //private void InitTreeView()
        //{
        //    long lngRootID = 1;
        //    if (IsPower != 0)
        //    {
        //        lngRootID = IsPower;
        //    }

        //    if (System.Configuration.ConfigurationSettings.AppSettings["SystemModel"] == "1")
        //    {
        //        //如果是租用的方式,只显示所在机构一层的部门树

        //        lngRootID = long.Parse(Session["UserOrgID"].ToString());
        //    }

        //    //long lngRootID = (long)Session["UserDeptID"];
        //    if (Session["OldDeptID"] != null)
        //    {
        //        lngCurrDeptID = long.Parse(Session["OldDeptID"].ToString());
        //    }
        //    else
        //    {
        //        lngCurrDeptID = long.Parse(Session["UserDeptID"].ToString());
        //    }

        //    if (lngRootID == 0)
        //    {
        //        return;
        //    }


        //    ODeptCollection dc = DeptControl.GetAllDeptCollection();


        //    tvDept.Nodes.Clear();
        //    TreeNode root = new TreeNode();

        //    root.Text = dc.GetODept(lngRootID).Name;
        //    root.Value = lngRootID.ToString();
        //    //root.ImageUrl = @"..\Images\flow_modify.ico";
        //    root.ImageUrl = @"..\Images\Flow\1.bmp";
        //    root.Expanded = true;
        //    tvDept.Nodes.Add(root);

        //    AddSubDepts(ref root, dc, lngRootID, "0.");
        //   // setSelectedExpand();
        //    tvDept.Attributes.Add("onclick", "if (typeof(Tree_Click) != 'undefined') { Tree_Click();}");
        //    tvDept.Attributes.Add("ondblclick", "if (typeof(Tree_DBClick) != 'undefined') { Tree_DBClick();}");


        //}

        private void AddSubDepts(ref TreeNode root, ODeptCollection dc, long lngID, string strIndex)
        {
            TreeNode node;
            int iPoint = 0;
            foreach (ODept d in dc)
            {
                if (d.ParentID == lngID && d.ID != d.ParentID)
                {
                    //当限制显示当前部门时判断
                    if (d.ID != this.CurrDeptID || this.LimitCurr == false)
                    {
                        node = new TreeNode();
                        node.Value = d.ID.ToString();
                        node.Text = d.Name;
                        node.Expanded = false;

                        if (d.IsTemp == 0)
                        {
                            node.ImageUrl = @"..\Images\FlowDesign\333.ico";
                        }
                        else
                        {
                            node.ImageUrl = @"..\Images\FlowDesign\333.ico";
                        }
                        AddSubDepts(ref node, dc, d.ID, strIndex + iPoint.ToString() + ".");
                        root.ChildNodes.Add(node);

                        iPoint++;

                    }
                }
            }
        }

        //#region 陈志文 2012-11-23
        private void LoadData_new()
        {
            try
            {
                //DataTable table = new DataTable();
                //table = DeptDP.Get_One_Depts(-1);       //得到所有所有父节点，放到DataTable中，这里默认根节点的父节为0
                //BindNode_noe(table, tvDept.Nodes);   //绑定所有的父节点     
                ////Page.RegisterStartupScript("redirect", "<script>redirect(1)</script>");
                hidQueryDeptID.Value = CtrDeptTree1.CurrentDeptId.ToString();

                btnQuery_Click();
            }
            catch (Exception ex)
            {
                throw;
            }
        }

        //private void BindNode_noe(DataTable table, TreeNodeCollection node)
        //{
        //    DataView dv = new DataView(table);
        //    TreeNode NewNode;
        //    foreach (DataRowView dr in dv)
        //    {
        //        NewNode = new TreeNode();

        //        NewNode.Text = dr["deptname"].ToString();
        //        NewNode.Value = dr["deptid"].ToString();
        //        int deptkind = Convert.ToInt32(dr["deptkind"].ToString());
        //        int istemp = Convert.ToInt32(dr["istemp"].ToString());
        //        if (deptkind == Convert.ToInt32(eO_DeptKind.eOrg))
        //        {
        //            NewNode.ImageUrl = RootImg;

        //        }
        //        else
        //        {
        //            if (istemp == 0)
        //            {
        //                NewNode.ImageUrl = NodeImg;
        //            }
        //            else
        //            {
        //                NewNode.ImageUrl = NodeImg;
        //            }
        //        }

        //        node.Add(NewNode);



        //        #region 二级菜单
        //        //查找第二级的菜单
        //        long lngRootID = long.Parse(Session["RootDeptID"].ToString());
        //        DataTable table1 = new DataTable();
        //        table1 = DeptDP.Get_One_Depts(lngRootID);
        //        DataView dv1 = new DataView(table1);
        //        TreeNode NewNode1;
        //        foreach (DataRowView dr1 in dv1)
        //        {
        //            NewNode1 = new TreeNode();

        //            NewNode1.Text = dr1["deptname"].ToString();
        //            NewNode1.Value = dr1["deptid"].ToString();
        //            int deptkind1 = Convert.ToInt32(dr1["deptkind"].ToString());
        //            int istemp1 = Convert.ToInt32(dr1["istemp"].ToString());
        //            if (deptkind1 == Convert.ToInt32(eO_DeptKind.eOrg))
        //            {
        //                NewNode1.ImageUrl = RootImg;

        //            }
        //            else
        //            {
        //                if (istemp1 == 0)
        //                {
        //                    NewNode1.ImageUrl = NodeImg;
        //                }
        //                else
        //                {
        //                    NewNode1.ImageUrl = NodeImg;
        //                }
        //            }

        //            NewNode1.Expanded = false;

        //            NewNode.ChildNodes.Add(NewNode1);
        //        }
        //        #endregion
        //    }
        //}
        //#endregion

        //protected void tvDept_SelectedNodeChanged(object sender, EventArgs e)
        //{
        //    //===zxl

        //    if (!string.IsNullOrEmpty(tvDept.SelectedValue))
        //    {
        //        deptId = long.Parse(tvDept.SelectedValue);

        //    }
        //    // 记录当前部门
        //    Session["OldDeptID"] = deptId;
        //    hidDeptID.Value = deptId.ToString();
        //    hidQueryDeptID.Value = hidDeptID.Value;
        //    txtDeptName.Text = DeptDP.GetDeptName(deptId);//获取部门名

        //    btnQuery_Click();

        //    txtDeptName.Text = tvDept.SelectedNode.Text;
        //    hidDeptID.Value = tvDept.SelectedValue;
        //    hidQueryDeptID.Value = tvDept.SelectedValue;


        //    #region 陈志文 2012-11-23
        //    TreeNode Node = tvDept.SelectedNode;
        //    string value = Node.Value;
        //    string deptvalue = Session["deptvalue"].ToString();

        //    if (deptvalue.IndexOf(value) < 0)
        //    {
        //        getDataNode(long.Parse(Node.Value), Node);
        //        deptvalue = deptvalue + "," + value;
        //        Session["deptvalue"] = deptvalue;
        //    }
        //    #endregion


        //}

        //#region 点击"+"图标触发节点事件 - 2013-03-29 @孙绍棕

        ///// <summary>
        ///// 点击"+"图标触发节点事件
        ///// </summary>
        ///// <param name="sender"></param>
        ///// <param name="e"></param>
        //protected void tvDept_TreeNodeExpanded(object sender, TreeNodeEventArgs e)
        //{

        //    long deptId = long.Parse(e.Node.Value);


        //    // 记录当前部门
        //    Session["OldDeptID"] = deptId;
        //    hidDeptID.Value = deptId.ToString();
        //    hidQueryDeptID.Value = hidDeptID.Value;
        //    txtDeptName.Text = DeptDP.GetDeptName(deptId);//获取部门名

        //    btnQuery_Click();

        //    txtDeptName.Text = e.Node.Text;
        //    hidDeptID.Value = e.Node.Value;
        //    hidQueryDeptID.Value = e.Node.Value;


        //    #region 陈志文 2012-11-23
        //    TreeNode Node = e.Node;
        //    string value = Node.Value;
        //    string deptvalue = Session["deptvalue"].ToString();

        //    if (deptvalue.IndexOf(value) < 0)
        //    {
        //        getDataNode(long.Parse(Node.Value), Node);
        //        deptvalue = deptvalue + "," + value;
        //        Session["deptvalue"] = deptvalue;
        //    }
        //    #endregion
        //}

        //#endregion

        //#region 陈志文 2012-11-23

        //private void getDataNode(long ParentId, TreeNode Node)
        //{
        //    BindNode(DeptDP.Get_One_Depts(ParentId), Node.ChildNodes);      //向结点填充数据.
        //}

        //private void BindNode(DataTable table, TreeNodeCollection node)
        //{
        //    DataView dv = new DataView(table);
        //    TreeNode NewNode;
        //    foreach (DataRowView dr in dv)
        //    {
        //        NewNode = new TreeNode();

        //        NewNode.Text = dr["deptname"].ToString();
        //        NewNode.Value = dr["deptid"].ToString();
        //        int deptkind = Convert.ToInt32(dr["deptkind"].ToString());
        //        int istemp = Convert.ToInt32(dr["istemp"].ToString());
        //        if (deptkind == Convert.ToInt32(eO_DeptKind.eOrg))
        //        {
        //            NewNode.ImageUrl = RootImg;

        //        }
        //        else
        //        {
        //            if (istemp == 0)
        //            {
        //                NewNode.ImageUrl = NodeImg;
        //            }
        //            else
        //            {
        //                NewNode.ImageUrl = NodeImg;
        //            }
        //        }

        //        NewNode.Expanded = false;
        //        node.Add(NewNode);
        //    }
        //}

        //#endregion

        #region Web 窗体设计器生成的代码
        //override protected void OnInit(EventArgs e)
        //{
        //    //
        //    // CODEGEN: 该调用是 ASP.NET Web 窗体设计器所必需的。

        //    //
        //    InitializeComponent();
        //    base.OnInit(e);
        //}
        #endregion
        //=======================================================



    }
}
