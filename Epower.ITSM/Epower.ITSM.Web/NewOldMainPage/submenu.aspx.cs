/*******************************************************************
 * ��Ȩ���У��Ƿ���Ϣ����
 * Description���˵�ҳ��
 * 
 * 
 * Create By  ��
 * Create Date��2010-04-10
 * *****************************************************************/
using System;
using System.Data;
using System.Configuration;
using System.Collections;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Web.UI.HtmlControls;
using Epower.DevBase.Organization.SqlDAL;

namespace Epower.ITSM.Web.NewOldMainPage
{
    public partial class submenu : BasePage
    {
        private string UserName = string.Empty;
        /// <summary>
        /// ҳ�����
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                //Response.Cache.AddValidationCallback(new HttpCacheValidateHandler(Validate), null);
                UserName = Session["UserName"].ToString();

                if (Request["mid"] != null)
                {
                    string strUrl = string.Empty;
                    string strmid = Request["mid"].ToString();
                    strUrl = "~/" + strmid + ".aspx";
                    this.SiteMapDataSource1.StartingNodeUrl = strUrl;

                    //  Page.Controls.Add(TreeView1);

                }
            }
            
        }


        public void Validate(HttpContext context, Object data, ref HttpValidationStatus status)
        {
            object obj = Session["RemoveSubMenuCach"];

            bool isV = true;
            //if(obj!=null && obj.GetType().Equals(typeof(Boolean)))
            if (obj != null)
            {
                isV = (bool)obj;
                if (isV == true)
                {
                    status = HttpValidationStatus.Invalid;
                    try
                    {
                        Session.Remove("RemoveSubMenuCach");
                    }
                    finally
                    {
                        
                    }
                }
                else
                {
                    status = HttpValidationStatus.Valid;
                }
            }
            else
            {
                status = HttpValidationStatus.Valid;
            }
            
        }
  
       
        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void TreeView1_TreeNodeDataBound(object sender, TreeNodeEventArgs e)
        {
            //�ڵ�
            SiteMapNode smn = (SiteMapNode)e.Node.DataItem;
            string strOpID = smn.ResourceKey;
            Epower.ITSM.SqlDAL.UIMethod ui = new Epower.ITSM.SqlDAL.UIMethod();

            string strTarget = smn["target"];

            if (strTarget != "")
            {
                e.Node.Target = strTarget;
            }
            
            //if (strOpID != null && strOpID.Length > 0)
            ui.CheckNodeRight(strOpID, e.Node, (Hashtable)Session["UserAllRights"], TreeView1, smn, UserName);            
        }

      

        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void TreeView1_DataBound(object sender, EventArgs e)
        {
            //�������Ӳ˵���ȫ���Ƴ��ĸ��˵����Ƴ�
            ClearNoSubItems1();

        }

        /// <summary>
        /// �������Ӳ˵���ȫ���Ƴ��ĸ��˵����Ƴ�
        /// </summary>
        private void ClearNoSubItems1()
        {
            //��û���Ӳ˵��ĵ�һ���˵�ɾ��


            TreeView lmenu = TreeView1;
            for (int i = lmenu.Nodes.Count - 1; i > -1; i--)
            {
                TreeNode item = lmenu.Nodes[i];
                ClearNoSubItems1(item);
                //����¼�ɾ����ϣ���ɾ���Լ�
                if (item.ChildNodes.Count == 0 && item.NavigateUrl == "")
                {
                    TreeView1.Nodes.Remove(item);
                }
            }

        }

        /// <summary>
        /// �ݹ����Ƿ����Ӳ˵������û���Ӳ˵������Ƴ����˵�
        /// </summary>
        /// <param name="pitem"></param>
        private void ClearNoSubItems1(TreeNode pitem)
        {
            TreeNode litem = pitem;
            //�ݹ�˵�
            for (int i = 0; i < litem.ChildNodes.Count; i++)
            {
                ClearNoSubItems1(pitem.ChildNodes[i]);
            }
            //���û���Ӳ˵������Ƴ����˵�
            if (pitem.ChildNodes.Count == 0 && pitem.NavigateUrl == "")
            {
                if (pitem.Parent != null)
                    pitem.Parent.ChildNodes.Remove(pitem);
            }
        }    
    }
}
