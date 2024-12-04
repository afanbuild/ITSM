/*******************************************************************
 * ��Ȩ���У�
 * Description���������ؼ�
 * 
 * 
 * Create By  ��
 * Create Date��
 * *****************************************************************/
namespace Epower.ITSM.Web.Controls
{
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
    /// <summary>
    ///		CtrDeptTree ��ժҪ˵����
    /// </summary>
    /// 

    public partial class CtrDeptTree : System.Web.UI.UserControl
    {

        private string strNodeIndex = "0";
        private long lngCurrDeptID = 0;


        /// <summary>
        /// �������ĸ߶�
        /// </summary>
        public System.Web.UI.WebControls.Unit TreeHeight
        {
            set { tvDept.Height = value; }
        }

        /// <summary>
        /// �������Ŀ��
        /// </summary>
        public System.Web.UI.WebControls.Unit TreeWidth
        {
            set { tvDept.Width = value; }
        }

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




        /// <summary>
        /// �Ƿ�����Ȩ��
        /// </summary>
        public long IsPower
        {
            get { return ViewState["IsPower"] == null ? 0 : StringTool.String2Long(ViewState["IsPower"].ToString()); }
            set { ViewState["IsPower"] = value; }
        }

        protected void Page_Load(object sender, System.EventArgs e)
        {
            if (Page.IsPostBack == false)
            {
                InitTreeView();
            }
        }

        private void InitTreeView()
        {
            long lngRootID = 1;
            if (IsPower != 0)
            {
                lngRootID = IsPower;
            }

            if (System.Configuration.ConfigurationSettings.AppSettings["SystemModel"] == "1")
            {
                //��������õķ�ʽ,ֻ��ʾ���ڻ���һ��Ĳ�����
                lngRootID = long.Parse(Session["UserOrgID"].ToString());
            }

            //long lngRootID = (long)Session["UserDeptID"];
            if (Session["OldDeptID"] != null)
            {
                lngCurrDeptID = long.Parse(Session["OldDeptID"].ToString());
            }
            else
            {
                lngCurrDeptID = long.Parse(Session["UserDeptID"].ToString());
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

            //if(lngCurrDeptID == lngRootID)
            //{
            //    strNodeIndex = root.GetNodeIndex();
            //}

            AddSubDepts(ref root, dc, lngRootID, "0.");
            
            tvDept.Attributes.Add("onclick", "if (typeof(Tree_Click) != 'undefined') { Tree_Click();}");
            tvDept.Attributes.Add("ondblclick", "if (typeof(Tree_DBClick) != 'undefined') { Tree_DBClick();}");
        }

        private void AddSubDepts(ref TreeNode root, ODeptCollection dc, long lngID, string strIndex)
        {
            TreeNode node;
            int iPoint = 0;
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
                        node.Expanded = true;                        
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
