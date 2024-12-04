/****************************************************************************
 * 
 * description:�豸���������ṹչʾ
 * 
 * 
 * 
 * Create by:
 * Create Date:2007-08-22
 * *************************************************************************/

using System;
using System.Data;
using System.Drawing;
using System.Web;
using System.Web.UI.WebControls;
using System.Web.UI.HtmlControls;
using Epower.DevBase.Organization;
using Epower.DevBase.Organization.Base;
//using Microsoft.Web.UI.WebControls;
using Epower.DevBase.BaseTools;

using Epower.ITSM.SqlDAL;

namespace Epower.ITSM.Web.Controls
{
    /// <summary>
    ///		ctrSubjecttree ��ժҪ˵����
    /// </summary>
    /// 
    public partial class ctrServertree : System.Web.UI.UserControl //: System.Web.UI.UserControl
    {
        #region ������
        private string strNodeIndex = "0";
        private long lngCurrSubjectID = 0;
        #endregion

        #region ������
        /// <summary>
        /// ��ʾ��������ʱ���Ƿ���������༰�¼�����
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
        /// ��ǰ��Ŀ���
        /// </summary>
        public long CurrSubjectID
        {
            get { return ViewState["CurrSubjectID"] == null ? 0 : StringTool.String2Long(ViewState["CurrSubjectID"].ToString()); }
            set { ViewState["CurrSubjectID"] = value; }
        }
        #endregion

        #region ������

        #region ҳ����� Page_Load
        /// <summary>
        /// ҳ�����
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void Page_Load(object sender, System.EventArgs e)
        {
            if (Page.IsPostBack == false)
            {
                InitTreeView();
            }
        }
        #endregion

        #region ��ʼ���� InitTreeView
        /// <summary>
        /// 
        /// </summary>
        private void InitTreeView()
        {
            long lngRootID = -1;
            if (Session["OldTemplateID"] != null)
            {
                lngCurrSubjectID = long.Parse(Session["OldTemplateID"].ToString());
            }
            DataTable dt = Equ_ServerDP.GetServers();
            tvSubject.Nodes.Clear();
            TreeNode root = new TreeNode();
            if (Equ_ServerDP.GetRootSubject().Rows.Count > 0)	//�����û��Ϊ�û�����Ӹ����࣬����ʾ��
            {
                root.Text = Equ_ServerDP.GetRootSubject().Rows[0]["TemplateName"].ToString();
            }
            else
            {
                Response.Write("<script>alert('�Բ���û����Ӹ��������ϵ����Ա��');</script>");
                return;
            }
            //zxl======
            // root.Text = lngRootID.ToString();
            root.Value = lngRootID.ToString();
            root.ImageUrl = @"..\Images\catalog.ico";
            root.Expanded = true;
            //=========zxl==�����
            tvSubject.Nodes.Add(root);


            //if (lngCurrSubjectID == lngRootID)
            //{
            //    strNodeIndex = root.GetNodeIndex();
            //}

            AddSubSubjects(ref root, dt, lngRootID, "0.");
            setSelectedExpand();
            tvSubject.Attributes.Add("onclick", "Tree_Click()");

        }
        #endregion

        #region ѡ��ڵ� setSelectedExpand
        /// <summary>
        /// ѡ��
        /// </summary>
        private void setSelectedExpand()
        {
            //�������󱣳ֽ����ڸ��ڵ�
            //try
            //{
            //    TreeNode node = tvSubject.GetNodeFromIndex(strNodeIndex);
            //    SetParentExpand(node);
            //    node.Expanded = true;

            //    tvSubject.SelectedNodeIndex = strNodeIndex;
            //}
            //catch
            //{
            //}

        }
        #endregion

        #region ���ø��ڵ� SetParentExpand
        /// <summary>
        /// ���ø��ڵ�
        /// </summary>
        /// <param name="node"></param>
        private void SetParentExpand(TreeNode node)
        {
            //string strIndex = ((TreeNode)node.Parent).GetNodeIndex();
            //if (strIndex != "0")
            //{
            //    TreeNode pnode = (TreeNode)node.Parent;
            //    SetParentExpand(pnode);
            //    pnode.Expanded = true;
            //}
        }
        #endregion

        #region �����ӽڵ� AddSubSubjects
        /// <summary>
        /// �����ӽڵ�
        /// </summary>
        /// <param name="root"></param>
        /// <param name="dt"></param>
        /// <param name="lngID"></param>
        /// <param name="strIndex"></param>
        private void AddSubSubjects(ref TreeNode root, DataTable dt, long lngID, string strIndex)
        {
            TreeNode node;
            int iPoint = 0;
            long lngParentID = 0;
            long lngSubjectID = 0;
            foreach (DataRow dr in dt.Rows)
            {
                lngParentID = long.Parse(dr["SERVICELEVELID"].ToString());
                lngSubjectID = long.Parse(dr["TEMPLATEID"].ToString());
                if (lngParentID == lngID && lngSubjectID != lngParentID)
                {
                    if (lngSubjectID != this.CurrSubjectID || this.LimitCurr == false)
                    {
                        node = new TreeNode();
                        node.Value = lngSubjectID.ToString();
                        node.Text = dr["TEMPLATENAME"].ToString();
                        node.Expanded = false;

                        node.ImageUrl = @"..\Images\left_cata.gif";

                        AddSubSubjects(ref node, dt, lngSubjectID, strIndex + iPoint.ToString() + ".");
                        root.ChildNodes.Add(node);

                        //if (lngSubjectID == lngCurrSubjectID)
                        //{
                        //    strNodeIndex = node.GetNodeIndex();
                        //    if (strNodeIndex.Length == 0)
                        //    {
                        //        strNodeIndex = strIndex + iPoint.ToString();
                        //    }
                        //}

                        iPoint++;

                    }
                }
            }
        }
        #endregion

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
        ///		�����֧������ķ��� - ��Ҫʹ�ô���༭��
        ///		�޸Ĵ˷��������ݡ�
        /// </summary>
        private void InitializeComponent()
        {

        }
        #endregion
        #endregion

        protected void tvSubject_SelectedNodeChanged(object sender, EventArgs e)
        {
            if (tvSubject.SelectedNode != null)
            {
                tvSubject.SelectedNode.Expand();
            }
        }

        protected void tvSubject_TreeNodeExpanded(object sender, TreeNodeEventArgs e)
        {

        }
    }
}
