/****************************************************************************
 * 
 * description:֪ʶ�����������ṹչʾ
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

namespace Epower.ITSM.Web.InformationManager
{
	/// <summary>
    ///		ctrSubjecttree ��ժҪ˵����
	/// </summary>
	/// 
    public partial class ctrSubjecttree : System.Web.UI.UserControl
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
        /// <summary>
        /// ֪ʶ������
        /// </summary>
        public bool InformationLimit
        {
            get
            {
                return ViewState["InformationLimit"] == null ? false : bool.Parse(ViewState["InformationLimit"].ToString());
            }
            set { ViewState["InformationLimit"] = value; }
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
			if(Page.IsPostBack == false)
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
            long lngRootID = 1;
            if (Session["OldSubjectID"] != null)
            {
                lngCurrSubjectID = long.Parse(Session["OldSubjectID"].ToString());
            }
            DataTable dt = Inf_SubjectDP.GetSubjects(InformationLimit);
            TreeView1.Nodes.Clear();
            //tvSubject.Nodes.Clear();
            TreeNode root = new TreeNode();
            if (Inf_SubjectDP.GetRootSubject().Rows.Count > 0)	//�����û��Ϊ�û�����Ӹ����࣬����ʾ��
            {
                root.Text = Inf_SubjectDP.GetRootSubject().Rows[0]["CatalogName"].ToString();
            }
            else
            {
                Response.Write("<script>alert('�Բ���û����Ӹ��������ϵ����Ա��');</script>");
                return;
            }
            root.Value  = lngRootID.ToString();
            root.ImageUrl = @"..\Images\catalog.ico";
            root.Expanded = true;
            //tvSubject.Nodes.Add(root);
            TreeView1.Nodes.Add(root);

            AddSubSubjects( root, dt, lngRootID, "0.");
            
            //tvSubject.Attributes.Add("onclick", "Tree_Click()");
            TreeView1.Attributes.Add("onclick", "Tree_Click()");
            //TreeView1.SelectedNodeStyle.ForeColor = Color.Red;

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
        private void AddSubSubjects(TreeNode root, DataTable dt, long lngID, string strIndex)
		{
			TreeNode node;
            int iPoint = 0;
            long lngParentID = 0;
            long lngSubjectID = 0;
			foreach(DataRow dr in dt.Rows)
			{
                lngParentID = long.Parse(dr["ParentID"].ToString());
                lngSubjectID = long.Parse(dr["CatalogID"].ToString());
                if (lngParentID == lngID && lngSubjectID != lngParentID)
				{
                    if (lngSubjectID != this.CurrSubjectID || this.LimitCurr == false)
                    {
                        node = new TreeNode();
                        node.Value = lngSubjectID.ToString();
                        node.Text = dr["CatalogName"].ToString();
                        node.Expanded = false;

                        node.ImageUrl = @"..\Images\left_cata.gif";
                        AddSubSubjects( node, dt, lngSubjectID, strIndex + iPoint.ToString() + ".");
                        root.ChildNodes.Add(node);

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
    }
}
