/*******************************************************************
 * 版权所有：
 * Description：用户组树展示控件
 * 
 * 
 * Create By  ：
 * Create Date：
 * *****************************************************************/
using System;
using System.Data;
using System.Drawing;
using System.Web;
using System.Web.UI.WebControls;
using System.Web.UI.HtmlControls;
//using Microsoft.Web.UI.WebControls;

using Epower.DevBase.Organization;
using Epower.DevBase.Organization.SqlDAL;
using Epower.DevBase.Organization.Base;

namespace Epower.ITSM.Web.Controls
{
    public partial class ctractortreeV52 : System.Web.UI.UserControl
    {
        /// <summary>
        /// 宽度
        /// </summary>
        public Unit Width
        {
            set
            {
                if (value != null)
                {
                   this.TreeView1.Width = value;
                }
                else
                {
                    TreeView1.Width = Unit.Parse("100%");
                }
            }
        }
        /// <summary>
        /// 高度
        /// </summary>
        public Unit Height
        {
            set
            {
                if (value != null)
                {
                    TreeView1.Height = value;
                }
                else
                {
                    TreeView1.Height = Unit.Parse("100%");
                }
            }
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            if (Page.IsPostBack == false)
            {
                InitTreeView();
            }
        }

        private void InitTreeView()
        {
            try
            {
                long lngSystemID = (long)Session["SystemID"];
                string strRangeID = Session["RangeID"].ToString();
                ActorCollection ac = ActorControl.GetAllActorCollection(lngSystemID, strRangeID);

                TreeNode root = new TreeNode();

                root.Text = Session["RootDeptName"].ToString() + "用户组";
                root.Value = "0";
                root.ImageUrl = @"..\Images\a2.ico";
                root.Expanded = true;

                AddSubActors(root, ac);
                TreeView1.Nodes.Add(root);
                TreeView1.Attributes.Add("onclick", "Tree_Click()");
                //tvActor.Attributes.Add("ondblclick", "if (typeof(Tree_DBClick) != 'undefined') { Tree_DBClick();}");
            }
            catch (Exception e)
            {
 
            }
        }

        private void AddSubActors(TreeNode root, ActorCollection ac)
        {
            TreeNode node;
            string strRangeID = "";
            string strOldRangeList = ",";
            string strDeptName = "";
            string strRootID = Session["RangeID"].ToString();

            TreeNode parentNode = root;
            int iFind = 0;
            try
            {
                foreach (oActor actor in ac)
                {
                    strRangeID = actor.RangeID;
                    strDeptName = actor.RangeDeptName;
                    if (strRangeID != strRootID)
                    {


                        if (("," + strRangeID) != strOldRangeList.Substring(strOldRangeList.LastIndexOf(",")))
                        {
                            //需要添加部门节点



                            //将指针回到上一个不等的ＲＡＮＧＥＩＤ的上一级

                            iFind = BackUpDifferenceDept(strRangeID, ref strOldRangeList, parentNode);
                            if (iFind == 0)
                            {
                                parentNode = root;
                            }

                            //添加部门节点
                            node = new TreeNode();
                            node.Value = "D_" + strRangeID;
                            node.Text = strDeptName;
                            string[] sArrTmp = strOldRangeList.Split(char.Parse(","));
                            if (sArrTmp.Length < 2)
                            {
                                node.Expanded = true;
                            }
                            else
                            {
                                node.Expanded = false;
                            }
                            node.ImageUrl = @"..\Images\a2.ico";
                            parentNode.ChildNodes.Add(node);

                            if (strOldRangeList.EndsWith(",") == true)
                            {
                                strOldRangeList = strOldRangeList + strRangeID;
                            }
                            else
                            {
                                strOldRangeList = strOldRangeList + "," + strRangeID;
                            }

                            parentNode = node;


                        }

                    }
                    else
                    {
                        parentNode = root;
                    }



                    node = new TreeNode();
                    node.Value = actor.ActorID.ToString();
                    node.Text = actor.ActorName;
                    node.Expanded = false;
                    node.ImageUrl = @"..\Images\a2.ico";
                    parentNode.ChildNodes.Add(node);


                }
            }
            catch (Exception e)
            {

            }
        }

        private int BackUpDifferenceDept(string strRangeID, ref string strList, TreeNode node)
        {
            string strUpList = "";
            string strUpRange = "";

            if (strList == "," || strRangeID == "")
                //退出递归
                return 0;
            else
            {
                strUpRange = strRangeID.Substring(0, strRangeID.Length - 6);
                strUpList = strList.Substring(0, strList.LastIndexOf(","));
                if (strUpList == "")
                    strUpList = ",";
                if (("," + strUpRange) == strList.Substring(strList.LastIndexOf(",")))
                {

                    return 1;
                }
                else
                {
                    node = (TreeNode)node.Parent;
                    strList = strUpList;
                    return BackUpDifferenceDept(strRangeID, ref strUpList, node);
                }
            }






        }
    }
}