/*******************************************************************
 * ��Ȩ���У�
 * Description��������չʾ�ؼ�
 * 
 * 
 * Create By  �� yxq
 * Create Date�� 2011-08-10
 * *****************************************************************/
using System;
using System.Data;
using System.Drawing;
using System.Web;
using System.Web.UI.WebControls;
using System.Web.UI.HtmlControls;
using Epower.DevBase.BaseTools;
using Epower.ITSM.Base;
using Microsoft.Web.UI.WebControls;
using Epower.DevBase.Organization.SqlDAL;
using Epower.ITSM.SqlDAL;

namespace Epower.ITSM.Web.Controls
{
	/// <summary>
	///		CtrCatalogTree ��ժҪ˵����
	/// </summary>
    public partial class ctrcatalogtreeNew : System.Web.UI.UserControl
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

        public long lngRootID
        {
            get
            {
                return ViewState["CurrlngRootID"] == null ? 0 : StringTool.String2Long(ViewState["CurrlngRootID"].ToString()); 
            }
            set {
                ViewState["CurrlngRootID"] = value;
            }
        }

		protected void Page_Load(object sender, System.EventArgs e)
		{
			// �ڴ˴������û������Գ�ʼ��ҳ��
			if(Page.IsPostBack == false)
			{
                if (Request.QueryString["RootID"] != null)
                {
                    lngRootID = long.Parse(Request.QueryString["RootID"] == "" ? "0" : Request.QueryString["RootID"]);
                }
				InitTreeView();
			}
		}

		private void InitTreeView()
		{
            OCatalogCollection cc = CatalogControl.GetAllCatalogCollectionbyRooID(lngRootID);

			tvCatalog.Nodes.Clear();
			Microsoft.Web.UI.WebControls.TreeNode root = new Microsoft.Web.UI.WebControls.TreeNode();
			if(cc.GetOCatalog(lngRootID) != null)	//�����û��Ϊ�û�����Ӹ����࣬����ʾ��
			{
				root.Text = cc.GetOCatalog(lngRootID).Name;
			}
			else
			{
				Response.Write("<script>alert('�Բ���û����Ӹ����࣬����ϵ����Ա��');</script>");
				return;
			}
			root.ID = lngRootID.ToString();
			//root.ImageUrl = @"..\Images\flow_modify.ico";
			root.ImageUrl = @"..\Images\catalog.ico";
			root.Expanded = true;
			tvCatalog.Nodes.Add(root);

			if(lngCurrCatalogID == lngRootID)
			{
				strNodeIndex = root.GetNodeIndex();
			}

            AddSubCatalogs(ref root, cc, lngRootID, "0.");
			setSelectedExpand();
			tvCatalog.Attributes.Add("onclick","Tree_Click()");
            tvCatalog.Attributes.Add("ondblclick", "if (typeof(Tree_DBClick) != 'undefined') { Tree_DBClick();}");
		}

		private void setSelectedExpand()
		{
			//�������󱣳ֽ����ڸ��ڵ�
			try
			{
				Microsoft.Web.UI.WebControls.TreeNode node = tvCatalog.GetNodeFromIndex(strNodeIndex);
				SetParentExpand(node);
				node.Expanded = true;

				tvCatalog.SelectedNodeIndex = strNodeIndex;
			}
			catch
			{
			}
			
		}

		private void SetParentExpand(Microsoft.Web.UI.WebControls.TreeNode node)
		{
			//string strCurrIndex = node.GetNodeIndex();
			string strIndex = ((Microsoft.Web.UI.WebControls.TreeNode)node.Parent).GetNodeIndex();
			if(strIndex != "0")
			{
				Microsoft.Web.UI.WebControls.TreeNode pnode = (Microsoft.Web.UI.WebControls.TreeNode)node.Parent;
				SetParentExpand(pnode);
				//Microsoft.Web.UI.WebControls.TreeNode tnode = tvCatalog.GetNodeFromIndex(strCurrIndex);
				pnode.Expanded = true;
			}
		}

        private void AddSubCatalogs(ref Microsoft.Web.UI.WebControls.TreeNode root, OCatalogCollection cc, long lngID, string strIndex)
        {
            Microsoft.Web.UI.WebControls.TreeNode node;
            int iPoint = 0;
            foreach (OCatalog c in cc)
            {
                if (c.ParentID == lngID && c.ID != c.ParentID)
                {
                    //��������ʾ��ǰ����ʱ�ж�
                    if (c.ID != this.CurrCatalogID || this.LimitCurr == false)
                    {
                        node = new Microsoft.Web.UI.WebControls.TreeNode();
                        node.ID = c.ID.ToString();
                        node.Text = c.Name;
                        node.Expanded = false;

                        node.ImageUrl = @"..\Images\left_data.gif";
                        AddSubCatalogs(ref node, cc, c.ID, strIndex + iPoint.ToString() + ".");
                        root.Nodes.Add(node);



                        if (c.ID == lngCurrCatalogID)
                        {
                            strNodeIndex = node.GetNodeIndex();
                            if (strNodeIndex.Length == 0)
                            {
                                strNodeIndex = strIndex + iPoint.ToString();
                            }
                        }

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
