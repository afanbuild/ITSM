//
// 文件名  ：ControlPage.CS
// 模块名称：
// 程序描述：
// 实现功能：用一个标准化的UserControl控制DataGrid的分页,替代DataGrid自身的分页功能
// 设计日期：2010-04-28
// 修改人  ：
// 修改日期：
// 版本    ：V1.0
//----------------------------------------------------------------
using System;
using System.Collections.Generic;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.HtmlControls;

namespace Epower.ITSM.Web.Controls
{
    public partial class ControlPageFoot : System.Web.UI.UserControl
    {
        protected void Page_Load(object sender, EventArgs e)
        {

        }
        public delegate void ControlPageFootDelegate();
        public ControlPageFootDelegate On_PageIndexChanged = null;

        private int recordCount;
        public int RecordCount
        {
            set
            {
                recordCount = value;
            }

        }

        #region ViewState

        public int PageCount
        {
            get
            {
                return (ViewState[this.ClientID.ToString() + "PageCount"] != null) ? (int)ViewState[this.ClientID.ToString() + "PageCount"] : 1;
            }
            set
            {
                ViewState[this.ClientID.ToString() + "PageCount"] = value;
            }
        }

        public int CurrentPage
        {
            get
            {
                if (ViewState[this.ClientID.ToString() + "CurrentPage"] != null)
                {
                    if (ViewState[this.ClientID.ToString() + "CurrentPage"].ToString() != "0")
                    {
                        return (int)ViewState[this.ClientID.ToString() + "CurrentPage"];
                    }
                    else
                        return 1;
                }
                else
                    return 1;
            }
            set
            {
                ViewState[this.ClientID.ToString() + "CurrentPage"] = value;
            }
        }

        /// <summary>
        /// 分页控件的每页记录数
        /// </summary>
        public int PageSize
        {
            get
            {
                return int.Parse(drpPageSize.SelectedItem.Value);
            }
            set
            {
                drpPageSize.SelectedIndex = drpPageSize.Items.IndexOf(drpPageSize.Items.FindByValue(value.ToString()));
            }
        }
        #endregion

        private int getSmallPageSize()
        {
            if (drpPageSize.Items == null || drpPageSize.Items.Count == 0)
            {
                return 0;
            }
            int result = int.MaxValue;
            for (int i = 0; i < drpPageSize.Items.Count; i++)
            {
                if (!string.IsNullOrEmpty(drpPageSize.Items[i].Value))
                {
                    if (result > int.Parse(drpPageSize.Items[i].Value))
                    {
                        result = int.Parse(drpPageSize.Items[i].Value);
                    }
                }
            }
            return (result == int.MaxValue) ? 0 : result;
        }

        public void Bind()
        {
            if (recordCount < getSmallPageSize())
            {
                base.Visible = false;
                return;
            }
            else
            {
                base.Visible = true;
            }

            #region bind
            PageCount = recordCount / PageSize;
            if (recordCount % PageSize != 0)
                PageCount = recordCount / PageSize + 1;
            if (CurrentPage > PageCount)
                CurrentPage = PageCount;
            int fromRecord = CurrentPage > 0 ? ((CurrentPage - 1) * PageSize + 1) : 0;
            int toRecord = CurrentPage * PageSize;
            if (toRecord > recordCount)
                toRecord = recordCount;
            LabRecordCount.Text = recordCount.ToString();
            LabCurrentPage.Text = CurrentPage.ToString();
            LabPageCount.Text = PageCount.ToString();

            if (CurrentPage < PageCount)
            {
                btnNext.Enabled = true;
                btnLast.Enabled = true;
            }
            else
            {
                btnNext.Enabled = false;
                btnLast.Enabled = false;
            }

            if (CurrentPage > 1)
            {
                btnFirst.Enabled = true;
                btnPre.Enabled = true;
            }
            else
            {
                btnFirst.Enabled = false;
                btnPre.Enabled = false;
            }
            #endregion
        }

        protected void btnFirst_Click(object sender, ImageClickEventArgs e)
        {
            if (null != On_PageIndexChanged)
            {
                CurrentPage = 1;
                On_PageIndexChanged();
            }
        }

        protected void btnPre_Click(object sender, ImageClickEventArgs e)
        {
            if (null != On_PageIndexChanged)
            {
                CurrentPage--;
                On_PageIndexChanged();
            }
        }

        protected void btnNext_Click(object sender, ImageClickEventArgs e)
        {
            if (null != On_PageIndexChanged)
            {
                CurrentPage++;
                On_PageIndexChanged();
            }
        }

        protected void btnLast_Click(object sender, ImageClickEventArgs e)
        {
            if (null != On_PageIndexChanged)
            {
                CurrentPage = PageCount;
                On_PageIndexChanged();
            }
        }

        protected void drpPageSize_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (null != On_PageIndexChanged)
            {
                CurrentPage = 1;
                On_PageIndexChanged();
            }
        }
    }
}