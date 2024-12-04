/*******************************************************************
 * 版权所有：深圳市非凡信息技术有限公司
 * 描述：复选框用户控件

 * 
 * 
 * 创建人：余向前
 * 创建日期：2013-05-22 
 * 
 * 修改日志：
 * 修改时间：
 * 修改描述：

 * *****************************************************************/
using System;
using System.Collections;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Xml.Linq;
using Epower.ITSM.Base;
using Epower.ITSM.SqlDAL;

namespace Epower.ITSM.Web.Controls
{
    public partial class ctrFlowCataCheckBox : System.Web.UI.UserControl
    {
        #region 变量定义区
        public string sApplicationUrl = Constant.ApplicationPath;
        private string sValue = "";
        private long lngRootID = 0;
        private string strMessage = "";

        #endregion

        #region 属性定义

        /// <summary>
        /// 获取或设置根节点
        /// </summary>
        public long RootID
        {
            get { return lngRootID; }
            set
            {
                lngRootID = value;
                HidRootID.Value = value.ToString();

                SetDboControlValue(ref chkBoxList, lngRootID);
            }
        }

        /// <summary>
        /// 获取或设置控件状态
        /// </summary>
        public eOA_FlowControlState ContralState
        {
            get
            {
                if (ViewState["eOA_FlowControlState"] != null)
                    return (eOA_FlowControlState)ViewState["eOA_FlowControlState"];
                else
                    return eOA_FlowControlState.eNormal;
            }
            set { ViewState["eOA_FlowControlState"] = value; }
        }

        /// <summary>
        /// 获取或设置控件的值
        /// </summary>
        public string CatelogValue
        {
            get
            {
                string returnValue = "";

                //循环获取所有选定复选框的值
                foreach (ListItem list in chkBoxList.Items)
                {
                    if (list.Selected)
                    {
                        returnValue += "," + list.Value;
                    }
                    else
                    {
                        returnValue += ",0";
                    }
                }

                return returnValue.Trim(',');
            }
            set
            {
                sValue = value;
                hidCatelogValue.Value = value;

                //给复选框列表赋初值
                if (chkBoxList.Items.Count > 0 && value.Trim() != string.Empty)
                {
                    if (value.Contains(","))
                    {
                        string[] arr = value.Split(',');
                        for (int i = 0; i < arr.Length; i++)
                        {
                            foreach (ListItem list in chkBoxList.Items)
                            {
                                if (list.Value == arr[i])
                                    list.Selected = true;
                            }
                        }
                    }
                    else
                    {
                        foreach (ListItem list in chkBoxList.Items)
                        {
                            if (list.Value == value)
                                list.Selected = true;
                        }
                    }
                }
            }
        }

        #endregion

        protected void Page_Load(object sender, EventArgs e)
        {
            if (ContralState != eOA_FlowControlState.eNormal)
            {
                chkBoxList.Enabled = false;
                if (ContralState == eOA_FlowControlState.eHidden)
                {
                    chkBoxList.Visible = false;
                }
            }
        }

        #region 设置下拉列表值 SetDboControlValue
        /// <summary>
        /// 设置下拉列表值
        /// </summary>
        /// <param name="chkBoxList">复选框对象</param>
        /// <param name="lngRootID">根ID</param>
        private void SetDboControlValue(ref CheckBoxList chkBoxList, long lngRootID)
        {
            ArrayList arrValues = new ArrayList();
            ArrayList arrTexts = new ArrayList();

            SetArrListValue(lngRootID, ref arrTexts, ref arrValues);
            //清空复选框内容项
            chkBoxList.Items.Clear();
            //重新给复选框添加内容项
            for (int i = 0; i < arrValues.Count; i++)
            {
                if (arrTexts[i].ToString().Trim() != "")
                    chkBoxList.Items.Add(new ListItem(arrTexts[i].ToString().Replace(System.Web.HttpUtility.HtmlDecode("&#160;"), "").Replace(System.Web.HttpUtility.HtmlDecode("&nbsp;"), "").Trim(), arrValues[i].ToString()));
            }

        }
        #endregion

        #region 设置列表 SetArrListValue
        /// <summary>
        /// 设置列表
        /// </summary>
        /// <param name="lngRootID"></param>
        /// <param name="arrTexts"></param>
        /// <param name="arrValues"></param>
        private void SetArrListValue(long lngRootID, ref ArrayList arrTexts, ref ArrayList arrValues)
        {
            DataTable dt = CatalogDP.GetBelowCatas(lngRootID);
            bool blnHasRoot = false;
            if (dt == null)
                return;
            if (dt.Rows.Count > 0)
            {
                //找根节点
                foreach (DataRow row in dt.Rows)
                {
                    if (row["catalogid"].ToString() == lngRootID.ToString())
                    {
                        arrTexts.Add("");
                        arrValues.Add("0");
                        blnHasRoot = true;
                        break;
                    }
                }
            }
            if (blnHasRoot == true)
            {
                //递归添加下级
                AddSubArrList(dt, lngRootID, 1, ref arrTexts, ref arrValues);
            }
        }
        #endregion

        #region 设置子列表 AddSubArrList
        /// <summary>
        /// 设置子列表 
        /// </summary>
        /// <param name="dt"></param>
        /// <param name="lngRootID"></param>
        /// <param name="Layer"></param>
        /// <param name="arrTexts"></param>
        /// <param name="arrValues"></param>
        private void AddSubArrList(DataTable dt, long lngRootID, int Layer, ref ArrayList arrTexts, ref ArrayList arrValues)
        {
            foreach (DataRow row in dt.Rows)
            {
                if (row["parentid"].ToString() == lngRootID.ToString())
                {
                    arrTexts.Add(row["CataLogName"].ToString());
                    arrValues.Add(row["catalogid"].ToString());
                    AddSubArrList(dt, long.Parse(row["catalogID"].ToString()), Layer + 1, ref arrTexts, ref arrValues);
                }
            }
        }
        #endregion
    }
}