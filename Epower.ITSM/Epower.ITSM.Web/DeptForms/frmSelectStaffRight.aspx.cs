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
using Epower.DevBase.Organization.SqlDAL;
using Epower.DevBase.Organization.Base;
using Epower.DevBase.BaseTools;

namespace Epower.ITSM.Web.Forms
{
	/// <summary>
	/// frmSelectStaffRight ��ժҪ˵����
	/// </summary>
	public partial class frmSelectStaffRight : BasePage
	{

      

        /// <summary>
        /// ���ݲ����ж��Ǹ�ҳ��
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
        #region ��ô������Ĳ��������жϸ�����Ŀؼ�id�����Ƶ���ͬ����
        /// <summary>
        /// ��ô������Ĳ��������жϸ�����Ŀؼ�id�����Ƶ���ͬ����
        /// </summary>
        public string Opener_ClientId
        {

            get
            {
                return (Request["Opener_ClientId"] == null) ? "" : Request["Opener_ClientId"].ToString().Replace("hidClientId_ForOpenerPage", "");
            }
        }
        #endregion

	
		protected void Page_Load(object sender, System.EventArgs e)
		{
			if(!IsPostBack)
			{
                //======zxl
                long deptId=0;
                if (!string.IsNullOrEmpty(tvDept.SelectedValue))
                {
                    deptId = long.Parse(tvDept.SelectedValue);
                }
                else 
                {
                    deptId = 1;
                }
            

				//long deptId = Convert.ToInt32(Request.Params.Get("DeptID"));
				LoadData(deptId);
                //InitTreeView();

                Session["deptvalue"] = "1";
                LoadData_new();
			}
		}

        private void LoadData_new()
        {
            try
            {
                DataTable table = new DataTable();
                table = DeptDP.Get_One_Depts(-1);       //�õ��������и��ڵ㣬�ŵ�DataTable�У�����Ĭ�ϸ��ڵ�ĸ���Ϊ0
                BindNode_noe(table, tvDept.Nodes);   //�����еĸ��ڵ�     
            }
            catch (Exception ex)
            {
                throw;
            }
        }

		private void LoadData(long lngDeptId)
		{
			DataTable dt=DeptControl.GetDeptUserList(lngDeptId);
			dlUsers.DataSource=dt.DefaultView;
			dlUsers.DataBind();
		}

		#region Web ������������ɵĴ���
		override protected void OnInit(EventArgs e)
		{
			//
			// CODEGEN: �õ����� ASP.NET Web ���������������ġ�
			//
		 //	InitializeComponent();
			base.OnInit(e);
		}
		
		/// <summary>
		/// �����֧������ķ��� - ��Ҫʹ�ô���༭���޸�
		/// �˷��������ݡ�
		/// </summary>
        //private void InitializeComponent()
        //{    

        //}
		#endregion
        //===========================���οؼ��Ĵ���===========
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

        /// <summary>
        ///  ����һ����
        /// </summary>
        public bool FirstLevel
        {
            get
            {
                if (ViewState["FirstLevel"] != null)
                    return bool.Parse(ViewState["FirstLevel"].ToString());
                else
                    return false;
            }
            set
            {
                ViewState["FirstLevel"] = value;
            }
        }

        private string strNodeIndex = "0";
        private long lngCurrDeptID = 0;



        /// <summary>
        /// ��ʾ��������ʱ���Ƿ���������ż��¼�����
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
        /// ��ǰ���ű��
        /// </summary>
        public long CurrDeptID
        {
            get { return ViewState["CurrDeptID"] == null ? 0 : StringTool.String2Long(ViewState["CurrDeptID"].ToString()); }
            set { ViewState["CurrDeptID"] = value; }
        }

        private void InitTreeView()
        {
            long lngRootID = long.Parse(Session["RootDeptID"].ToString());
            if (Session["OldDeptID"] != null)
            {
                lngCurrDeptID = long.Parse(Session["OldDeptID"].ToString());

            }
            else
            {
                lngCurrDeptID = lngRootID;
            }
            //CurrDeptID = lngCurrDeptID;


            tvDept.Nodes.Clear();
            if (lngRootID == 0)
            {
                return;
            }

            //lngCurrDeptID = 1132;
            ODeptCollection dc = DeptControl.GetAllDeptCollection();

            TreeNode root = new TreeNode();
            root.Text = dc.GetODept(lngRootID).Name;
            root.Value = lngRootID.ToString();
            //root.NavigateUrl = "javascript:var x='" + root.Value + "@" + root.Text + "';";

            root.Expanded = true;
            tvDept.Nodes.Add(root);

            if (dc.GetODept(lngRootID).DeptKind == eO_DeptKind.eOrg)
            {
                root.ImageUrl = RootImg;
            }
            else
            {
                if (dc.GetODept(lngRootID).IsTemp == 0)
                {
                    root.ImageUrl = NodeImg;
                }
                else
                {
                    root.ImageUrl = NodeImg;
                }
            }

            AddSubDepts(ref root, dc, lngRootID);

            tvDept.SelectedNodeStyle.BackColor = Color.Yellow;
            tvDept.SelectedNodeStyle.ForeColor = Color.Red;

        }

        private void AddSubDepts(ref TreeNode root, ODeptCollection dc, long lngID)
        {
            TreeNode node;
            foreach (ODept d in dc)
            {
                if (d.ParentID == lngID && d.ID != d.ParentID)
                {
                    //��������ʾ��ǰ����ʱ�ж�
                    if (d.ID != this.CurrDeptID || this.LimitCurr == false)
                    {
                        node = new TreeNode();
                        node.Value = d.ID.ToString();
                        node.Text = d.Name;
                        //node.NavigateUrl = "javascript:var x='" + node.Value + "@" + node.Text + "';";
                        node.Expanded = false;
                        root.ChildNodes.Add(node);
                        //if(d.ID == lngCurrDeptID)
                        //{
                        //    strNodeIndex = node.GetNodeIndex();
                        //}
                        if (d.DeptKind == eO_DeptKind.eOrg)
                        {
                            node.ImageUrl = RootImg;
                        }
                        else
                        {
                            if (d.IsTemp == 0)
                            {
                                node.ImageUrl = NodeImg;
                            }
                            else
                            {
                                node.ImageUrl = NodeImg;
                            }
                        }
                        if (!FirstLevel)  //֪ʶ��չʾ���Ƿ�����һ������ 
                        {
                            AddSubDepts(ref node, dc, d.ID);
                        }

                    }
                }
            }
        }



        private void BindNode_noe(DataTable table, TreeNodeCollection node)
        {
            DataView dv = new DataView(table);
            TreeNode NewNode;
            foreach (DataRowView dr in dv)
            {
                NewNode = new TreeNode();

                NewNode.Text = dr["deptname"].ToString();
                NewNode.Value = dr["deptid"].ToString();
                int deptkind = Convert.ToInt32(dr["deptkind"].ToString());
                int istemp = Convert.ToInt32(dr["istemp"].ToString());
                if (deptkind == Convert.ToInt32(eO_DeptKind.eOrg))
                {
                    NewNode.ImageUrl = RootImg;

                }
                else
                {
                    if (istemp == 0)
                    {
                        NewNode.ImageUrl = NodeImg;
                    }
                    else
                    {
                        NewNode.ImageUrl = NodeImg;
                    }
                }

                node.Add(NewNode);

                #region �����˵�
                //���ҵڶ����Ĳ˵�
                long lngRootID = long.Parse(Session["RootDeptID"].ToString());
                DataTable table1 = new DataTable();
                table1 = DeptDP.Get_One_Depts(lngRootID);
                DataView dv1 = new DataView(table1);
                TreeNode NewNode1;
                foreach (DataRowView dr1 in dv1)
                {
                    NewNode1 = new TreeNode();

                    NewNode1.Text = dr1["deptname"].ToString();
                    NewNode1.Value = dr1["deptid"].ToString();
                    int deptkind1 = Convert.ToInt32(dr1["deptkind"].ToString());
                    int istemp1 = Convert.ToInt32(dr1["istemp"].ToString());
                    if (deptkind1 == Convert.ToInt32(eO_DeptKind.eOrg))
                    {
                        NewNode1.ImageUrl = RootImg;

                    }
                    else
                    {
                        if (istemp1 == 0)
                        {
                            NewNode1.ImageUrl = NodeImg;
                        }
                        else
                        {
                            NewNode1.ImageUrl = NodeImg;
                        }
                    }

                    NewNode.ChildNodes.Add(NewNode1);
                }
                #endregion
            }
        }

        private void BindNode(DataTable table, TreeNodeCollection node)
        {
            DataView dv = new DataView(table);
            TreeNode NewNode;
            foreach (DataRowView dr in dv)
            {
                NewNode = new TreeNode();

                NewNode.Text = dr["deptname"].ToString();
                NewNode.Value = dr["deptid"].ToString();
                int deptkind = Convert.ToInt32(dr["deptkind"].ToString());
                int istemp = Convert.ToInt32(dr["istemp"].ToString());
                if (deptkind == Convert.ToInt32(eO_DeptKind.eOrg))
                {
                    NewNode.ImageUrl = RootImg;

                }
                else
                {
                    if (istemp == 0)
                    {
                        NewNode.ImageUrl = NodeImg;
                    }
                    else
                    {
                        NewNode.ImageUrl = NodeImg;
                    }
                }
                node.Add(NewNode);
            }
        }



        private void getDataNode(long ParentId, TreeNode Node)
        {
            BindNode(DeptDP.Get_One_Depts(ParentId), Node.ChildNodes);      //�����������.
        }

        protected void tvDept_SelectedNodeChanged(object sender, EventArgs e)
        {
            if (!string.IsNullOrEmpty(tvDept.SelectedValue)) 
            {
                LoadData(long.Parse(tvDept.SelectedValue));  
              //  hidUserID
            }


            TreeNode Node = tvDept.SelectedNode;
            string value = Node.Value;
            string deptvalue = Session["deptvalue"].ToString();

            if (deptvalue.IndexOf(value) < 0)
            {
                getDataNode(long.Parse(Node.Value), Node);
                deptvalue = deptvalue + "," + value;
                Session["deptvalue"] = deptvalue;
            }
        }
        //========zxl==
	}
}
