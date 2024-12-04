using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using Epower.ITSM.SqlDAL;
using Epower.ITSM.Base;

namespace Epower.ITSM.Web.Controls
{
    public partial class CtrCataSelectShowInpText : System.Web.UI.UserControl
    {
        private string _CatalogId = string.Empty;
        /// <summary>
        /// 常用类别id
        /// </summary>
        public string CatalogId
        {
            get { return _CatalogId; }
            set
            {
                _CatalogId = value;

                //初始化下拉框
                InitData();

                if (selCatalog.Items.Count > 0)
                {
                    hidSelCatalogId.Value = selCatalog.Items[selCatalog.SelectedIndex].Value;
                    lblCatalog.Text = selCatalog.Items[selCatalog.SelectedIndex].Text;
                }

            }
        }

        private string _MustInput = "false";
        /// <summary>
        /// 是否必填
        /// </summary>
        public string MustInput
        {
            get { return _MustInput; }
            set { _MustInput = value; }
        }

        private string _SelToolTip = "信息项";
        /// <summary>
        /// 必选提示文本
        /// </summary>
        public string SelToolTip
        {
            get { return _SelToolTip; }
            set
            {
                _SelToolTip = value;
            }
        }

        /// <summary>
        /// 文本框显示条件（下拉列表选中值）
        /// </summary>
        public string TextShowCatalogId
        {
            get { return hidCatalogId.Value; }
            set { hidCatalogId.Value = value; }
        }

        private string _TextMustInput = "true";
        /// <summary>
        /// 文本是否必填
        /// </summary>
        public string TextMustInput
        {
            get { return _TextMustInput; }
            set { _TextMustInput = value; }
        }

        private string _ToolTipString = "信息项";
        /// <summary>
        /// 必填提示文本
        /// </summary>
        public string ToolTipString
        {
            get { return _ToolTipString; }
            set { 
                _ToolTipString = value;
                hidText.Value = value;
            }
        }

        private string _TextControlWidth = "90%";
        /// <summary>
        /// 文本框控件宽度
        /// </summary>
        public string TextControlWidth
        {
            get { return _TextControlWidth; }
            set { _TextControlWidth = value; }
        }

        private string _SelValue = string.Empty;
        /// <summary>
        /// 获取或设置下拉列表选中值
        /// </summary>
        public string SelValue
        {
            get 
            {
                return hidSelCatalogId.Value;
                //return selCatalog.Value; 
            }
            set
            {
                _SelValue = value;
                hidSelCatalogId.Value = value;

                if (selCatalog.Items.Count <= 0)
                    InitData();                

                selCatalog.SelectedIndex = selCatalog.Items.IndexOf(selCatalog.Items.FindByValue(value.ToString()));

                lblCatalog.Text = selCatalog.Items[selCatalog.SelectedIndex].Text;
            }
        }

        /// <summary>
        /// 获取选中列表选中文本
        /// </summary>
        public string SelText
        {
            get { return selCatalog.Items[selCatalog.SelectedIndex].Text;                            
            }
        }

        /// <summary>
        /// 获取文本框值
        /// </summary>
        public string TextValue
        {
            get { return txtContent.Text == ToolTipString ? "" : txtContent.Text; }
            set
            {
                txtContent.Text = value;
                lblContent.Text = value;
            }
        }


        private eOA_FlowControlState cState = eOA_FlowControlState.eNormal;
        /// <summary>
        /// 控件状态
        /// </summary>
        public eOA_FlowControlState ContralState
        {
            get { return cState; }
            set { cState = value; }
        }


        protected void Page_Load(object sender, EventArgs e)
        {

            if (!string.IsNullOrEmpty(SelValue) && SelValue != "0")
            {   
                if (TextShowCatalogId.Contains(SelValue))
                    trContent.Attributes.Remove("style");
            }
            if (string.IsNullOrEmpty(TextValue))
            {
                txtContent.Text = ToolTipString;
            }

            if (TextMustInput.ToLower() == "true")
            {
                lblStar.Visible = true;
            }

            if (MustInput.ToLower() == "true")
            {
                sp.Visible = true;
                string strThisMsg = "";
                strThisMsg = selCatalog.ClientID + ">" + SelToolTip;
                this.Page.ClientScript.RegisterStartupScript(this.GetType(), selCatalog.ClientID, "<script type=\"text/javascript\" >if(typeof(sarrvalidlist) == 'undefined'){ var sarrvalidlist = new Array(1);}  sarrvalidlist[0] = sarrvalidlist[0] + '|' + '" + strThisMsg + "';</script>");
            }

            if (cState != eOA_FlowControlState.eNormal)
            {
                lblCatalog.Visible = true;
                selCatalog.Visible = false;
                lblContent.Visible = true;
                txtContent.Visible = false;
                lblStar.Visible = false;
                sp.Visible = false;
                if (cState == eOA_FlowControlState.eHidden)
                {
                    lblCatalog.Text = "--";
                    lblContent.Text = "--";
                }
            }
        }

        private void InitData()
        {
            if (!string.IsNullOrEmpty(CatalogId))
            {
                DataTable dt = CatalogDP.GetBelowCatas(long.Parse(CatalogId));
                selCatalog.Items.Clear();
                selCatalog.Items.Add(new ListItem("",""));
                foreach (DataRow dr in dt.Rows)
                {
                    if (CatalogId != dr["catalogid"].ToString())
                        selCatalog.Items.Add(new ListItem(dr["catalogname"].ToString(), dr["catalogid"].ToString()));
                }
            }

            //if (selCatalog.Items.Count > 0)
            //    hidSelCatalogId.Value = selCatalog.Items[selCatalog.SelectedIndex].Value;

            //if (TextMustInput.ToLower() == "true")
            //{
            //    lblStar.Visible = true;
            //}

            //if (!string.IsNullOrEmpty(_CatalogId) && _SelValue!="0")
            //{
            //    selCatalog.Items.FindByValue(_SelValue).Selected = true;
            //    lblCatalog.Text = selCatalog.Items[selCatalog.SelectedIndex].Text;
            
            //if (TextShowCatalogId == _SelValue)
            //    trContent.Attributes.Remove("style");
            //}

            //if (string.IsNullOrEmpty(TextValue))
            //{
            //    txtContent.Text = ToolTipString;
            //}
        }
    }
}