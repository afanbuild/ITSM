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
using Epower.DevBase.Organization;
using Epower.DevBase.Organization.Base;
using Epower.DevBase.BaseTools;
using System.Text;
using System.Drawing;

namespace Epower.ITSM.Web.mydestop
{
    public partial class frmperson_zxl :Page
    {
        /// <summary>
        /// 人员类型，选择人员或者服务人员
        /// </summary>
        protected string Type
        {
            get
            {
                if (Request["Type"] != null)
                    return Request["Type"].ToString();
                else
                    return "-1";
            }
        }
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

        public string Opener_ClientId
        {
            set
            {
                ViewState["Opener_ClientId"] = value;
            }
            get
            {
                return (ViewState["Opener_ClientId"] == null) ? "" : ViewState["Opener_ClientId"].ToString();
            }
        }


        protected void Page_Load(object sender, System.EventArgs e)
        {
            if (Page.IsPostBack == false)
            {
                InitTreeView();
            }

            if (!IsPostBack)
            {
                if (Request["Opener_ClientId"] != null)
                {
                    Opener_ClientId = Request["Opener_ClientId"].ToString().Replace("hidClientId_ForOpenerPage", "");
                }
            }
        }

        private void LoadData(long lngDeptId)
        {
            DataTable dt = new DataTable();
            dt = DeptControl.GetDeptUserList(lngDeptId);

            lsbStaff.DataSource = dt.DefaultView;
            lsbStaff.DataTextField = "Name";
            lsbStaff.DataValueField = "userid";
            lsbStaff.DataBind();
            lsbStaff.Visible = true;
        }
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
           // lsbStaff.DataSource;
        }

        protected void lsbStaff_SelectedIndexChanged(object sender, EventArgs e)
        {

            StringBuilder sbText = new StringBuilder();

            if (TypeFrm == "QuestHouse")
            {
                //=============
                sbText.Append("window.opener.document.getElementById('" + Opener_ClientId + "Lb_execByName').innerText='"+lsbStaff.SelectedItem.Text+"';");
                sbText.Append("window.opener.document.getElementById('" + Opener_ClientId + "Lb_execByName').innerText='"+lsbStaff.SelectedItem.Text+"';");
                sbText.Append("window.opener.document.getElementById('" + Opener_ClientId + "HidexecByName').value='" + lsbStaff.SelectedItem.Text+"';");
                sbText.Append("window.opener.document.getElementById('" + Opener_ClientId + "HidexecById').value='" + lsbStaff.SelectedValue+"';");
                //===========

               DataTable dt= Epower.DevBase.Organization.SqlDAL.UserDP.getExecUser(long.Parse(lsbStaff.SelectedValue));
                string sReturnValue="";
                 if (dt.Rows.Count > 0)
                 {
                    sReturnValue = dt.Rows[0]["LOGINNAME"].ToString() + "," + dt.Rows[0]["Name"].ToString() + "," + dt.Rows[0]["MOBILE"].ToString() + "," + dt.Rows[0]["TELNO"].ToString() + "," + dt.Rows[0]["DeptName"].ToString() + "," + dt.Rows[0]["DeptId"].ToString(); 
                 }
                 string[] str = sReturnValue.Split(',');
                 sbText.Append("window.opener.document.getElementById('" + Opener_ClientId + "HiddeptName').value='"+str[4]+"';");
               //  sbText.Append("window.opener.document.getElementById('" + Opener_ClientId + "lb_execDeptname').value=rturnValue[4];");
                 sbText.Append("window.opener.document.getElementById('" + Opener_ClientId + "lb_execDeptname').innerText= '" + str[4] + "';");
                 sbText.Append("window.opener.document.getElementById('" + Opener_ClientId + "HiddeptId').value= '" + str[5] + "';");

                 sbText.Append("window.opener.document.getElementById('" + Opener_ClientId + "HidexecByPhone').value= '" + str[2] + "';");
               //  sbText.Append("window.opener.document.getElementById('" + Opener_ClientId + "lb_execByPhone').value= rturnValue[2];");

                 sbText.Append("window.opener.document.getElementById('" + Opener_ClientId + "lb_execByPhone').innerText= '" + str[2] + "';");

                 sbText.Append("window.opener.document.getElementById('" + Opener_ClientId + "HidexecByGP').value= '" + str[0] + "';");

               //  sbText.Append("window.opener.document.getElementById('" + Opener_ClientId + "lb_execByGH').value= rturnValue[0];");
                 sbText.Append("window.opener.document.getElementById('" + Opener_ClientId + "lb_execByGH').innerText= '" + str[0] + "';");
                //==================

            }
            else 
            {
                sbText.AppendFormat("window.opener.document.all.{0}.value='{1}';", "txtObjectName", lsbStaff.SelectedItem.Text);   //设备名称
                sbText.AppendFormat("window.opener.document.all.{0}.value='{1}';", "txtObjectId", lsbStaff.SelectedValue);
            }
            sbText.AppendFormat("window.close();");
            // 向客户端发送
            ScriptManager.RegisterStartupScript(UpdatePanel1, this.GetType(), DateTime.Now.Ticks.ToString(), sbText.ToString(), true);

        }

        //===================================================================

        private string strNodeIndex = "0";
		private long lngCurrDeptID = 0;


		/// <summary>
		/// 设置树的高度
		/// </summary>
		public System.Web.UI.WebControls.Unit TreeHeight
		{
			set{tvDept.Height=value;}
		}

        /// <summary>
        /// 设置树的宽度
        /// </summary>
        public System.Web.UI.WebControls.Unit TreeWidth
        {
            set { tvDept.Width = value; }
        }

		/// <summary>
		/// 显示部门树的时候是否包含本部门及下级部门
		/// </summary>
		public bool LimitCurr
		{
			get
			{
				return ViewState["LimitCurr"]==null?false:bool.Parse(ViewState["LimitCurr"].ToString());
			}
			set{ViewState["LimitCurr"]=value;}
		}

		/// <summary>
		/// 当前部门编号
		/// </summary>
		public long CurrDeptID
		{
			get{return ViewState["CurrDeptID"]==null?0:StringTool.String2Long(ViewState["CurrDeptID"].ToString());}
			set{ViewState["CurrDeptID"]=value;}
		}




        /// <summary>
        /// 是否设置权限
        /// </summary>
        public long IsPower
        {
            get { return ViewState["IsPower"] == null ? 0 : StringTool.String2Long(ViewState["IsPower"].ToString()); }
            set { ViewState["IsPower"] = value; }
        }

		private void InitTreeView()
		{
			long lngRootID = 1;
            if (IsPower != 0)
            {
                lngRootID = IsPower;
            }

			if( System.Configuration.ConfigurationSettings.AppSettings["SystemModel"] == "1")
			{
				//如果是租用的方式,只显示所在机构一层的部门树
				lngRootID = long.Parse(Session["UserOrgID"].ToString());
			}

			//long lngRootID = (long)Session["UserDeptID"];
			if(Session["OldDeptID"]!= null)
			{
				lngCurrDeptID = long.Parse(Session["OldDeptID"].ToString());				
			}
			else
			{
				lngCurrDeptID =long.Parse(Session["UserDeptID"].ToString());
			}

            if (lngRootID == 0)
            {
                return;
            }


			ODeptCollection dc = DeptControl.GetAllDeptCollection();


			tvDept.Nodes.Clear();
			TreeNode root = new TreeNode();
			root.Text = dc.GetODept(lngRootID).Name;
			root.Value = lngRootID.ToString();
			//root.ImageUrl = @"..\Images\flow_modify.ico";
			root.ImageUrl = @"..\Images\Flow\1.bmp";
			root.Expanded = true;
			tvDept.Nodes.Add(root);
            tvDept.SelectedNodeStyle.ForeColor = Color.Red;
            tvDept.SelectedNodeStyle.BackColor = Color.Yellow;

			AddSubDepts(ref root,dc,lngRootID,"0.");

		}

		private void AddSubDepts(ref TreeNode root,ODeptCollection dc,long lngID,string strIndex)
		{
			TreeNode node;
			int iPoint = 0;
			foreach(ODept d in dc)
			{
				if(d.ParentID == lngID && d.ID != d.ParentID)
				{
					//当限制显示当前部门时判断
					if(d.ID != this.CurrDeptID || this.LimitCurr == false)
					{
						node = new TreeNode();
						node.Value = d.ID.ToString();
						node.Text = d.Name;
						node.Expanded = false;

						if(d.IsTemp == 0)
						{
							node.ImageUrl = @"..\Images\FlowDesign\333.ico";
						}
						else
						{
							node.ImageUrl = @"..\Images\FlowDesign\333.ico";
						}
						AddSubDepts(ref node,dc,d.ID,strIndex + iPoint.ToString() + ".");
						root.ChildNodes.Add(node);

						iPoint++;

					}
				}
			}
		}

        protected void tvDept_SelectedNodeChanged(object sender, EventArgs e)
        {
            if (!string.IsNullOrEmpty(tvDept.SelectedValue))
            {
                LoadData(Convert.ToInt32(this.tvDept.SelectedValue));
            }
        }



    }
}
