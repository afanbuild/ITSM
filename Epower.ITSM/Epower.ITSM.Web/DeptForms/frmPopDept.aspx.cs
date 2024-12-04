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
using Epower.DevBase.Organization.SqlDAL;
using Epower.DevBase.Organization.Base;

namespace Epower.ITSM.Web.Forms
{
	/// <summary>
	/// frmPopDept ��ժҪ˵����
	/// </summary>
	public partial class frmPopDept : BasePage
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


        private long mlngCurrDeptID = 1;
        protected long lngCurrDeptID
        {
            get
            {
                if (Request.QueryString["CurrDeptID"] != null)
                {
                    if (Request.QueryString["CurrDeptID"].Length > 0)
                        mlngCurrDeptID = long.Parse(Request.QueryString["CurrDeptID"]);
                    else
                        mlngCurrDeptID = long.Parse(Session["UserDeptID"].ToString());
                }
                else
                {
                    mlngCurrDeptID = long.Parse(Session["UserDeptID"].ToString());
                }
                return mlngCurrDeptID;
            }
        }
        /// <summary>
        /// ��ô������Ĳ����ж��ĸ���ҳ�洫�����ġ�
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
        /// ��ô������Ĳ��������жϸ�ҳ���id����
        /// </summary>
        public string Opener_ClientId
        {

            get
            {
                return (Request["Opener_ClientId"] == null) ? "" : Request["Opener_ClientId"].ToString().Replace("hidClientId_ForOpenerPage", "");
            }
        }
        /// <summary>
        /// ��ȡ��ҳ�洫�����İ�ťid
        /// </summary>
        public string ObjID
        {
            get { return String.IsNullOrEmpty(Request["objID"]) ? "" : Request["objID"].ToString(); }
        }


		protected void Page_Load(object sender, System.EventArgs e)
		{
            CtrDeptTree.SelectedChangeEvent += new Epower.ITSM.Web.DeptControls.CtrDeptTree.SelectedChangeHandler(CtrDeptTree_SelectedChangeEvent);

            if (!Page.IsPostBack)
            {
                Session["deptvalue"] = "1";
                LoadData_new();
            }
		}

        void CtrDeptTree_SelectedChangeEvent(object sender, TreeNode node)
        {
            if (node != null && node.Value != null)
            {
                hidDeptID.Value = node.Value + "@" + node.Text;
                deptname.Text = "ѡ��Ĳ��ţ�" + node.Text;
            }
        }

		#region Web ������������ɵĴ���
		override protected void OnInit(EventArgs e)
		{
			//
			// CODEGEN: �õ����� ASP.NET Web ���������������ġ�
			//
			InitializeComponent();
			base.OnInit(e);
		}
		
		/// <summary>
		/// �����֧������ķ��� - ��Ҫʹ�ô���༭���޸�
		/// �˷��������ݡ�
		/// </summary>
		private void InitializeComponent()
		{    

		}
		#endregion




        private void LoadData_new()
        {


            try
            {
                //DataTable table = new DataTable();
                //table = DeptDP.Get_One_Depts(-1);       //�õ��������и��ڵ㣬�ŵ�DataTable�У�����Ĭ�ϸ��ڵ�ĸ���Ϊ0
                //BindNode_noe(table, tvDept.Nodes);   //�����еĸ��ڵ�     
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

        //        #region �����˵�
        //        //���ҵڶ����Ĳ˵�
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

        //            NewNode.ChildNodes.Add(NewNode1);
        //        }
        //        #endregion
        //    }
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
        //        node.Add(NewNode);
        //    }
        //}

        //private void getDataNode(long ParentId, TreeNode Node)
        //{
        //    BindNode(DeptDP.Get_One_Depts(ParentId), Node.ChildNodes);      //�����������.
        //}

        //protected void tvDept_SelectedNodeChanged(object sender, EventArgs e)
        //{

        //    TreeNode Node = tvDept.SelectedNode;
        //    string value = Node.Value;
        //    string deptvalue = Session["deptvalue"].ToString();

        //    if (deptvalue.IndexOf(value) < 0)
        //    {
        //        getDataNode(long.Parse(Node.Value), Node);
        //        deptvalue = deptvalue + "," + value;
        //        Session["deptvalue"] = deptvalue;
        //    }

        //    hidDeptID.Value = Node.Value + "@" + Node.Text;
        //    deptname.Text = "ѡ��Ĳ��ţ�" + Node.Text;
        //}
        

        protected void btnSubmit_Click(object sender, EventArgs e)
        {
            Response.Write(hidDeptID.Value );
        }
	}
}
