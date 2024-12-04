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

namespace Epower.ITSM.Web.EquipmentManager
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
            if (Session["OldEQSubectID"] != null)
            {
                lngCurrSubjectID = long.Parse(Session["OldEQSubectID"].ToString());
            }
            DataTable dt = Equ_SubjectDP.GetSubjects();
            tvSubject.Nodes.Clear();
            TreeNode root = new TreeNode();
            if (Equ_SubjectDP.GetRootSubject().Rows.Count > 0)	//�����û��Ϊ�û�����Ӹ����࣬����ʾ��
            {
                root.Text = Equ_SubjectDP.GetRootSubject().Rows[0]["CatalogName"].ToString();
            }
            else
            {
                Response.Write("<script>alert('�Բ���û����Ӹ��������ϵ����Ա��');</script>");
                return;
            }
            root.Value = lngRootID.ToString();
            root.ImageUrl = @"..\Images\catalog.ico";
            root.Expanded = true;
            tvSubject.Nodes.Add(root);

            AddSubSubjects(ref root, dt, lngRootID, "0.");
            
            tvSubject.Attributes.Add("onclick", "Tree_Click()");

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
                        AddSubSubjects(ref node, dt, lngSubjectID, strIndex + iPoint.ToString() + ".");
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
