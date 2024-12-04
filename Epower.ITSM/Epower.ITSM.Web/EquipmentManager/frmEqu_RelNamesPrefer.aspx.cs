using System;
using System.Collections;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Web.UI.HtmlControls;
using System.Xml.Linq;
using Epower.ITSM.Log;
using Epower.ITSM.SqlDAL.EquipmentManager;
using Epower.DevBase.BaseTools;

namespace Epower.ITSM.Web.EquipmentManager
{
    public partial class frmEqu_RelNamesPrefer : System.Web.UI.Page
    {

        #region start here.
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                BindRelPreferList();
            }
        }
        #endregion


        #region bind data
        /// <summary>
        /// 显示用户视角偏好
        /// </summary>
        protected void BindRelPreferList()
        {
            Equ_RelNameDP equRelNameDP = new Equ_RelNameDP();
            DataTable dt = equRelNameDP.GetAll();

            dgPreferList.DataSource = dt;
            dgPreferList.DataBind();
        }
        #endregion

        #region  control event-handler on page.
        /// <summary>
        /// 保存视角偏好设置. 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void btnSaveChanges_Click(object sender, EventArgs e)
        {
            if (deptTree.UserID <= 0)
            {
                PageTool.MsgBox(Page, "请选择人员。");
                return;
            }

            Int32 intUserId;

            Boolean isOk = Int32.TryParse(hfUserId.Value, out intUserId);
            if (!isOk) return;

            /* 取该用户的使用偏好 */
            System.Collections.Generic.List<Equ_RelPreference> listPrefer
                = Equ_RelPreference.Load(intUserId);

            /* 在 dgPreferList 中检索选中项. 根据选中状态移除或增加用户的视角偏好.
             * 注意: 以下代码的算法是: 只存储用户未选中的视角. 因为默认是全部
             *       选中状态.
             */
            foreach (DataGridItem item in dgPreferList.Items)
            {
                CheckBox chkPrefer = item.Cells[2].FindControl("chkprefer") as CheckBox;
                Int32 relKeyId = Int32.Parse(item.Cells[0].Text.Trim());
                Int32 intIdx = listPrefer.FindIndex(delegate(Equ_RelPreference obj)
                {
                    if (obj.RelKeyId.Equals(relKeyId)) return true;

                    return false;
                });

                if (chkPrefer.Checked)
                {
                    if (intIdx > -1)
                    {
                        listPrefer[intIdx].Delete();

                        listPrefer.RemoveAt(intIdx);
                    }
                }
                else
                {
                    if (intIdx < 0)
                    {//新增
                        Equ_RelPreference equRelPrefer = new Equ_RelPreference();
                        equRelPrefer.UserId = intUserId;
                        equRelPrefer.RelKeyId = relKeyId;
                        equRelPrefer.Save();

                        listPrefer.Add(equRelPrefer);
                    }
                }
            }


            PageTool.MsgBox(Page, "已保存更改。");
        }

        /// <summary>
        /// 查询用户的视角偏好, 绑定到 [dgPreferList] 上显示.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void btnSearch_Click(object sender, EventArgs e)
        {
            literalAlert.Text = String.Empty;
            if (deptTree.UserID <= 0) return;
            /* 取用户编号 */
            Int32 intUserId = Int32.Parse(deptTree.UserID.ToString());

            /* 取消上次选择 */
            CancelChecked();

            /* 取该用户的使用偏好 */
            System.Collections.Generic.List<Equ_RelPreference> listPrefer
                = Equ_RelPreference.Load(intUserId);

            /* 在显示面板上显示 */
            foreach (DataGridItem item in dgPreferList.Items)
            {
                if (item.ItemType == ListItemType.AlternatingItem
                    || item.ItemType == ListItemType.Item)
                {
                    CheckBox chkPrefer = item.FindControl("chkprefer") as CheckBox;
                    Int32 intRelKeyId = Int32.Parse(item.Cells[0].Text);

                    Equ_RelPreference equRelPrefer = listPrefer.Find(delegate(Equ_RelPreference obj)
                    {
                        if (obj.RelKeyId == intRelKeyId) return true;

                        return false;
                    });

                    if (equRelPrefer != null)
                    {
                        chkPrefer.Checked = false;
                    }
                    else { chkPrefer.Checked = true; }
                }
            }

            /* 存储选择的用户编号 */
            hfUserId.Value = intUserId.ToString();
        }

        /// <summary>
        /// 重置请求, 返回资产关联视角页
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void btnReturn_Click(object sender, EventArgs e)
        {
            Response.Redirect("frmEqu_RelNamesMain.aspx");
        }
        #endregion

        #region extensin method.
        /// <summary>
        /// 取消视角的选择
        /// </summary>
        private void CancelChecked()
        {
            foreach (DataGridItem item in dgPreferList.Items)
            {
                if (item.ItemType == ListItemType.AlternatingItem
                    || item.ItemType == ListItemType.Item)
                {
                    CheckBox chkPrefer = item.FindControl("chkprefer") as CheckBox;
                    chkPrefer.Checked = false;
                }
            }
        }
        #endregion

    }
}
