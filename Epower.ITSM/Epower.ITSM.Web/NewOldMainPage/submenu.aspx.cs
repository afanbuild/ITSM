/*******************************************************************
 * 版权所有：非凡信息技术
 * Description：菜单页面
 * 
 * 
 * Create By  ：
 * Create Date：2010-04-10
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
        /// 页面加载
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
            //节点
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
            //将所有子菜单项全部移除的父菜单项移除
            ClearNoSubItems1();

        }

        /// <summary>
        /// 将所有子菜单项全部移除的父菜单项移除
        /// </summary>
        private void ClearNoSubItems1()
        {
            //把没有子菜单的第一级菜单删除


            TreeView lmenu = TreeView1;
            for (int i = lmenu.Nodes.Count - 1; i > -1; i--)
            {
                TreeNode item = lmenu.Nodes[i];
                ClearNoSubItems1(item);
                //如果下级删除完毕，则删除自己
                if (item.ChildNodes.Count == 0 && item.NavigateUrl == "")
                {
                    TreeView1.Nodes.Remove(item);
                }
            }

        }

        /// <summary>
        /// 递归检查是否有子菜单，如果没有子菜单，则移除父菜单
        /// </summary>
        /// <param name="pitem"></param>
        private void ClearNoSubItems1(TreeNode pitem)
        {
            TreeNode litem = pitem;
            //递归菜单
            for (int i = 0; i < litem.ChildNodes.Count; i++)
            {
                ClearNoSubItems1(pitem.ChildNodes[i]);
            }
            //如果没有子菜单，则移除父菜单
            if (pitem.ChildNodes.Count == 0 && pitem.NavigateUrl == "")
            {
                if (pitem.Parent != null)
                    pitem.Parent.ChildNodes.Remove(pitem);
            }
        }    
    }
}
