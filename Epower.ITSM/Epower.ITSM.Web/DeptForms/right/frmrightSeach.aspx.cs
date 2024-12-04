using System;
using System.Collections;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Web;
using System.Web.SessionState;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.HtmlControls;
using Epower.DevBase.Organization;
using Epower.DevBase.BaseTools;
using Epower.DevBase.Organization.Base;
using Epower.DevBase.Organization.SqlDAL;

namespace Epower.ITSM.Web.Forms.right
{
    /// <summary>
    /// frmRight 的摘要说明。
    /// </summary>
    public partial class frmrightSeach : BasePage
    {
        protected System.Web.UI.WebControls.Button cmdAdd;
        protected void Page_Load(object sender, System.EventArgs e)
        {
            cmdDelete.Attributes.Add("onclick", "if(!confirm('删除将无法恢复,确定要删除吗？')){return false;}");

            if (!IsPostBack)
            {
                LoadObjectType();
                uTitle.Title = "权限维护";

                lnkbtndept_Click(null, null);
            }
            ControlPageRight.On_PostBack += new EventHandler(ControlPageRight_On_PostBack);
            ControlPageRight.DataGridToControl = dgRight;
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
            this.dgRight.ItemCreated += new System.Web.UI.WebControls.DataGridItemEventHandler(this.dgRight_ItemCreated);
            this.dgRight.ItemDataBound += new System.Web.UI.WebControls.DataGridItemEventHandler(this.dgRight_ItemDataBound);

        }
        #endregion

        protected void cmdFind_Click(object sender, System.EventArgs e)
        {
            bool isall = true;
            if (hidSelect.Value == "0")
            {
                isall = false;
            }
            LoadData(isall);
            BindData();
        }
        /// <summary>
        /// 操作项id
        /// </summary>
        public string  operateId
        {
            get
            {
                if (Request["operateId"] != null)
                {
                    return Request["operateId"].ToString();
                }
                else
                {
                    return "";
                }
            }
            
        }

        private void LoadObjectType()
        {
            #region 对象类别
            ListItem lt = new ListItem("", "0");
            dpdObjectType.Items.Add(lt);

            lt = new ListItem("部门", ((int)eO_RightObject.eDeptRight).ToString());
            dpdObjectType.Items.Add(lt);

            lt = new ListItem("人员", ((int)eO_RightObject.eUserRight).ToString());
            dpdObjectType.Items.Add(lt);

            lt = new ListItem("用户组", ((int)eO_RightObject.eActorRight).ToString());
            dpdObjectType.Items.Add(lt);
            #endregion
        }
        //加载操作列表,并缓存
        /// <summary>
        /// 
        /// </summary>
        /// <param name="isall"></param>
        private void LoadData(bool isall)
        {
            // 修改人：ken
            // 修改时间：2005-09-20
            // 修改备注：
            // 修改方式：加入系统ID
            string sWhere = string.Empty;
            long lngSystemID = (long)Session["SystemID"];
            string strRangeID = Session["RangeID"].ToString();

            DataTable dt = new DataTable();
            sWhere = " and sysid = " + lngSystemID.ToString();

                long lngOpID = 0;
                if (Request["operateId"] != null)
                    lngOpID = StringTool.String2Long(Request["operateId"].ToString());
                if (lngOpID != 0)
                    sWhere += "And a.OperateId=" + lngOpID.ToString();


                if (txtObjectID.Text.Trim() != "" || dpdObjectType.SelectedValue != "0")
                {
                    int nObjectType = StringTool.String2Int(dpdObjectType.SelectedValue);
                    long lngObjectId = StringTool.String2Long(txtObjectID.Text);
                
                    if (lngObjectId != 0)
                    {
                        sWhere += " And a.ObjectId=" + lngObjectId.ToString();
                    }
                    if ((int)nObjectType != 0)
                    {
                        sWhere += " And a.ObjectType=" + ((int)nObjectType).ToString();
                    }                  
                }
                dt = RightDP.GetRightList(sWhere, isall, strRangeID);

           
            //缓存数据列表
            Session["RIGHT_EDIT"] = dt;
        }

        //加载操作列表
        private void BindData()
        {
            dgRight.DataSource = ((DataTable)Session["RIGHT_EDIT"]).DefaultView;
            dgRight.DataBind();
        }

        #region 删除


        /// <summary>
        /// 删除一个权限项
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void cmdDelete_Click(object sender, System.EventArgs e)
        {
            foreach (DataGridItem item in dgRight.Items)
            {
                if (((CheckBox)item.Cells[0].FindControl("chkSelect")).Checked)
                {
                    string sId = ((Label)item.Cells[2].FindControl("labRightID")).Text.ToString();
                    RightEntity re = new RightEntity();
                    re.RightID = long.Parse(sId);
                    re.Delete();
                }
            }

            bool isall = true;
            if (hidSelect.Value == "0")
            {
                isall = false;
            }
            LoadData(isall);
            BindData();

            long lngUserID = (long)Session["UserID"];
            Session["UserAllRights"] = RightDP.getUserRightTable(lngUserID);

            //PageTool.AddJavaScript(this,"window.parent.topFrame.location.reload(); ");


        }


        #endregion

        private void ControlPageRight_On_PostBack(object sender, EventArgs e)
        {
            bool isall = true;
            if (hidSelect.Value == "0")
            {
                isall = false;
            }
            LoadData(isall);
            BindData();
        }

        protected void cmdFind1_Click(object sender, System.EventArgs e)
        {
            bool isall = true;
            if (hidSelect.Value == "0")
            {
                isall = false;
            }
            LoadData(isall);
            BindData();
        }

        private void dgRight_ItemDataBound(object sender, System.Web.UI.WebControls.DataGridItemEventArgs e)
        {
            string strNameList = "";
            if (e.Item.ItemType == ListItemType.Item || e.Item.ItemType == ListItemType.AlternatingItem)
            {
                strNameList = "";
                string strDeptList = e.Item.Cells[8].Text.Trim();
                if (!strDeptList.Replace("&nbsp;", "").Equals(""))//Deleted
                {
                    strNameList = DeptDP.GetDeptListNames(strDeptList.Trim());
                    if (strNameList != "")
                    {
                        e.Item.Cells[6].Text = e.Item.Cells[6].Text + "(扩展：" + strNameList + ")";
                    }

                }


            }
        }

        private void dgRight_ItemCreated(object sender, System.Web.UI.WebControls.DataGridItemEventArgs e)
        {
            if (e.Item.ItemType == ListItemType.Header)
            {
                DataGrid dg = (DataGrid)sender;
                for (int i = 0; i < e.Item.Cells.Count; i++)
                {
                    // (DataView)e.Item.NamingContainer;
                    if (i >= 1 && i < 7)
                    {
                        int j = 0;
                        string strUserName = Session["UserName"].ToString().ToLower();
                        if (hidSelect.Value != "0" && strUserName != "sa")
                        {
                            j = i - 2;
                        }
                        else
                        {
                            j = i - 0;
                        }
                        //第3列到6列排序   2007-11-10 绑定列数值型很难排序. 所以不排第2 列了
                        if (i != 2)
                        {
                            e.Item.Cells[i].Attributes.Add("onclick", "sortTable('" + dg.ClientID + "'," + j.ToString() + ",0);");
                        }
                        else
                        {
                            //第2列按数值排序
                            e.Item.Cells[i].Attributes.Add("onclick", "sortTable('" + dg.ClientID + "'," + j.ToString() + ",1);");
                        }
                    }
                }
            }
        }

        /// <summary>
        /// 全部权限
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void lnkbtnall_Click(object sender, EventArgs e)
        {
            LoadData(true);
            BindData();

            string strUserName = Session["UserName"].ToString().ToLower();
            if (strUserName != "sa")
            {
                cmdDelete.Visible = false;
                dgRight.Columns[0].Visible = false;
                dgRight.Columns[1].Visible = false;
            }
            else
            {
                cmdDelete.Visible = true;
                dgRight.Columns[0].Visible = true;
                dgRight.Columns[1].Visible = true;
            }
        }

        /// <summary>
        /// 本部门管理员维护的权限
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void lnkbtndept_Click(object sender, EventArgs e)
        {
            LoadData(false);
            BindData();

            string strUserName = Session["UserName"].ToString().ToLower();
            if (strUserName != "sa")
            {
                cmdDelete.Visible = true;
                dgRight.Columns[0].Visible = true;
                dgRight.Columns[1].Visible = true;
            }
        }


    }
}
