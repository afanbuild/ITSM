/*******************************************************************
 * ��Ȩ���У�
 * Description��������չʾ�ؼ�
 * 
 * 
 * Create By  ��
 * Create Date��
 * *****************************************************************/
using System;
using System.Data;
using System.Drawing;
using System.Web;
using System.Web.UI.WebControls;
using System.Web.UI.HtmlControls;
using Epower.DevBase.BaseTools;
using Epower.ITSM.Base;
using Epower.DevBase.Organization.SqlDAL;
using Epower.ITSM.SqlDAL;

namespace Epower.ITSM.Web.Controls
{
    /// <summary>
    ///		CtrCatalogTree ��ժҪ˵����
    /// </summary>
    public partial class CtrCatalogTree : System.Web.UI.UserControl
    {

        private string strNodeIndex = "0";
        private long lngCurrCatalogID = 0;

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
        /// ��ǰ������
        /// </summary>
        public long CurrCatalogID
        {
            get { return ViewState["CurrCatalogID"] == null ? 0 : StringTool.String2Long(ViewState["CurrCatalogID"].ToString()); }
            set { ViewState["CurrCatalogID"] = value; }
        }


        protected void Page_Load(object sender, System.EventArgs e)
        {
            // �ڴ˴������û������Գ�ʼ��ҳ��
            if (Page.IsPostBack == false)
            {
                InitTreeView();
            }
        }

        private void InitTreeView()
        {
            long lngRootID = 1;

            if (System.Configuration.ConfigurationSettings.AppSettings["SystemModel"] == "1")
            {
                //��������õķ�ʽ,ֻ��ʾ���ڻ���һ��Ĳ�����
                lngRootID = long.Parse(Session["UserOrgID"].ToString());
            }

            if (Session["OldCatalogID"] != null)
            {
                lngCurrCatalogID = long.Parse(Session["OldCatalogID"].ToString());
            }

            OCatalogCollection cc = CatalogControl.GetAllCatalogCollection();

            tvCatalog.Nodes.Clear();
            TreeNode root = new TreeNode();
            if (cc.GetOCatalog(lngRootID) != null)	//�����û��Ϊ�û�����Ӹ����࣬����ʾ��
            {
                root.Text = cc.GetOCatalog(lngRootID).Name;
            }
            else
            {
                Response.Write("<script>alert('�Բ���û����Ӹ����࣬����ϵ����Ա��');</script>");
                return;
            }
            root.Value = lngRootID.ToString();
            root.ImageUrl = @"..\Images\catalog.ico";
            root.Expanded = true;
            tvCatalog.Nodes.Add(root);

            AddSubCatalogs(ref root, cc, lngRootID, "0.");
            setSelectedExpand();
            tvCatalog.Attributes.Add("onclick", "Tree_Click()");
            tvCatalog.Attributes.Add("ondblclick", "if (typeof(Tree_DBClick) != 'undefined') { Tree_DBClick();}");
        }

        private void setSelectedExpand()
        {

            TreeNode node = tvCatalog.SelectedNode;

            if (node != null)
                SetParentExpand(node);

        }

        private void SetParentExpand(TreeNode node)
        {
            //�ݹ����� չ����ǰѡ�нڵ�����и��ڵ�
            TreeNode pnode = node.Parent;
            if (pnode != null)
            {
                pnode.Expanded = true;
                SetParentExpand(pnode);
            }

        }

        private void AddSubCatalogs(ref TreeNode root, OCatalogCollection cc, long lngID, string strIndex)
        {
            TreeNode node;
            int iPoint = 0;
            foreach (OCatalog c in cc)
            {
                if (c.ParentID == lngID && c.ID != c.ParentID)
                {
                    //��������ʾ��ǰ����ʱ�ж�
                    if (c.ID != this.CurrCatalogID || this.LimitCurr == false)
                    {
                        node = new TreeNode();
                        node.Value = c.ID.ToString();
                        node.Text = c.Name;
                        node.Expanded = false;

                        //���õ�ǰѡ�еĽڵ�   yxq 2013-1-29
                        if (c.ID == lngCurrCatalogID)
                        {
                            node.Selected = true;
                        }

                        node.ImageUrl = @"..\Images\left_cata.gif";
                        AddSubCatalogs(ref node, cc, c.ID, strIndex + iPoint.ToString() + ".");
                        root.ChildNodes.Add(node);

                        iPoint++;

                    }
                }
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
        ///		�����֧������ķ��� - ��Ҫʹ�ô���༭��
        ///		�޸Ĵ˷��������ݡ�
        /// </summary>
        private void InitializeComponent()
        {

        }
        #endregion
    }
}
