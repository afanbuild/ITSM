/*******************************************************************
 * 版权所有：
 * Description：流程分类下拉列表控件
 * 
 * 
 * Create By  ：
 * Create Date：
 * *****************************************************************/
using System;
using System.Data;
using System.Configuration;
using System.Collections;
using System.Web;
using System.Drawing;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Web.UI.HtmlControls;
using Epower.ITSM.Base;
using Epower.ITSM.SqlDAL;

namespace Epower.ITSM.Web.EquipmentManager
{
    public partial class ctrFlowEquCategory : System.Web.UI.UserControl
    {

        #region 变量定义区
        private string sValue = "";

        private bool blnMust = false;
        private long lngRootID = 0;
        private string strMessage = "";
        private long lngCatelogID = 0;
        private int iIndex = 0;
        private bool blnRemark = false;
        #endregion


        #region 属性定义

        /// <summary>
        /// 是否一定要输入
        /// </summary>
        public bool MustInput
        {
            set
            {
                blnMust = value;

            }
            get { return blnMust; }
        }

        public bool ShowRemark
        {
            set
            {
                blnRemark = value;
            }
        }

        /// <summary>
        /// 宽度
        /// </summary>
        public Unit Width
        {
            set
            {
                if (value != null)
                {
                    ddlCate1.Width = value;
                }
                else
                {
                    ddlCate1.Width = Unit.Parse("70%");
                }
            }
        }


        /// <summary>
        /// 设置未输入时的提示信息
        /// </summary>
        public string TextToolTip
        {
            set { strMessage = value; }
        }

        /// <summary>
        /// 下拉控件的客户端ID
        /// </summary>
        public string ClientID
        {
            get { return ddlCate1.ClientID; }
        }

        /// <summary>
        /// 分类根节点
        /// </summary>
        public long RootID
        {
            get { return lngRootID; }
            set
            {
                lngRootID = value;

                SetDboControlValue(ref ddlCate1, lngRootID, lngCatelogID);
                //当时dropDownList 控件时的处理
                if (ddlCate1.Items.Count > 0)
                    labCate1.Text = ddlCate1.SelectedItem.Text.Replace(System.Web.HttpUtility.HtmlDecode("&nbsp;"), "");


                if (ContralState != eOA_FlowControlState.eNormal)
                {
                    labCate1.Visible = true;
                    ddlCate1.Visible = false;
                    rWarning.Visible = false;
                    if (ContralState == eOA_FlowControlState.eHidden)
                    {
                        labCate1.Text = "--";
                        lblMessage.Visible = false;
                    }
                }
            }
        }

        /// <summary>
        /// 当前选择节点Index
        /// </summary>
        public int CurIndex
        {
            get { return iIndex; }
            set { iIndex = value; }
        }

        /// <summary>
        /// 设置只读状态时的标签颜色
        /// </summary>
        public Color ForColor
        {
            set
            {
                labCate1.ForeColor = value;
                ddlCate1.ForeColor = value;
            }
        }


        /// <summary>
        /// 控件状态
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

        #region 获得 控件的 value ID 值
        /// <summary>
        /// 获得 控件的 value ID 值
        /// </summary>
        public long CatelogID
        {
            get
            {
                #region dropDownlist 控件
                //dropDownlist 控件
                if (ddlCate1.Items.Count > 0)
                    return long.Parse(ddlCate1.SelectedItem.Value);
                else
                    return -1;
                #endregion

            }
            set
            {
                lngCatelogID = value;
                hidCateID.Value = Convert.ToString(value);

                #region dropDownList控件
                //dropDownList控件
                if (ddlCate1.Items.Count == 0)
                {
                    SetDboControlValue(ref ddlCate1, lngRootID, lngCatelogID);
                }
                ddlCate1.SelectedIndex = ddlCate1.Items.IndexOf(ddlCate1.Items.FindByValue(value.ToString()));
                #endregion

                lblMessage.Text = "";
            }
        }

        #endregion 

        #region 获得控件的  ValueText 值
        /// <summary>
        /// 获得控件的  ValueText 值
        /// </summary>
        public string CatelogValue
        {
            get
            {
                if (ddlCate1.Items.Count > 0)
                    return ddlCate1.SelectedItem.Text.Trim().Replace(System.Web.HttpUtility.HtmlDecode("&#160;"), "").Replace(System.Web.HttpUtility.HtmlDecode("&nbsp;"), "");
                else
                    return string.Empty;

            }
            set
            {
                sValue = value;
                ddlCate1.SelectedIndex = ddlCate1.Items.IndexOf(ddlCate1.Items.FindByText(value));
            }
        }
        #endregion 

        /// <summary>
        /// 更改时客户端脚本
        /// </summary>
        public string OnChangeScript
        {
            set { ViewState["OnChangeScript"] = value; }
            get { return ViewState["OnChangeScript"] == null ? string.Empty : ViewState["OnChangeScript"].ToString(); }
        }

        #endregion

        public event EventHandler mySelectedIndexChanged;

        void DrpDictionary_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (mySelectedIndexChanged != null)

                mySelectedIndexChanged(this, new EventArgs()); //激活SelectIndexChanged事件 
        }
    
        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void Page_Load(object sender, EventArgs e)
        {
            //ddlCate1.Attributes.Add("onchange", "SelecteChanged()"); 
            if (mySelectedIndexChanged != null)
            {
                ddlCate1.AutoPostBack = true;
                ddlCate1.SelectedIndexChanged += new EventHandler(DrpDictionary_SelectedIndexChanged);
            }
            if (blnRemark)
            {
                lblMessage.Visible = true;
            }

            if (Page.IsPostBack == false)
            {
                if (lngRootID != 0)
                {
                    //没有设置根属性 则不做任何事情
                    #region
                    //当等于 DropDownList 是的处理
                    if (ddlCate1.Items.Count > 0)
                        labCate1.Text = ddlCate1.SelectedItem.Text.Replace(System.Web.HttpUtility.HtmlDecode("&nbsp;"), "");

                    if (blnMust == true)
                    {
                        //添加公共校验基础资料

                        string strThisMsg = "";
                        strThisMsg = ddlCate1.ClientID + ">" + strMessage + ">" + lngRootID.ToString();
                        this.Page.ClientScript.RegisterStartupScript(this.GetType(), ddlCate1.ClientID, "<script>if(typeof(sarrvalidlist) == 'undefined'){ var sarrvalidlist = new Array(1);}  sarrvalidlist[0] = sarrvalidlist[0] + '|' + '" + strThisMsg + "';</script>");
                    }
                    #endregion

                    if (ContralState != eOA_FlowControlState.eNormal)
                    {
                        labCate1.Visible = true;
                        ddlCate1.Visible = false;
                        rWarning.Visible = false;
                        if (ContralState == eOA_FlowControlState.eHidden)
                        {
                            labCate1.Text = "--";
                            lblMessage.Visible = false;
                        }
                    }

                }
              
                if (OnChangeScript.Length > 0)
                {
                    ddlCate1.Attributes.Add("onchange", "CateSelecteChanged(this);" + OnChangeScript);
                }
                else
                {
                    ddlCate1.Attributes.Add("onchange", "CateSelecteChanged(this);");
                }
            }
            if (blnMust == false)
            {
                rWarning.Visible = false;
            }
        }


        #region 设置下拉列表值 SetDboControlValue
        /// <summary>
        /// 设置下拉列表值
        /// </summary>
        /// <param name="ddl"></param>
        /// <param name="lngRootID"></param>
        /// <param name="lngValue"></param>
        private void SetDboControlValue(ref DropDownList ddl, long lngRootID, long lngValue)
        {
            ArrayList arrValues = new ArrayList();
            ArrayList arrTexts = new ArrayList();

            SetArrListValue(lngRootID, ref arrTexts, ref arrValues);
            ddl.Items.Clear();

            for (int i = 0; i < arrValues.Count; i++)
            {
                ddl.Items.Add(new ListItem(arrTexts[i].ToString(), arrValues[i].ToString()));
            }
            //设置当前值
            for (int i = 0; i < ddl.Items.Count; i++)
            {
                if (CurIndex != 0)
                {
                    ddl.SelectedIndex = CurIndex;
                }
                if (ddl.Items[i].Value == lngValue.ToString())
                {
                    ddl.SelectedIndex = i;
                    break;
                }
                if (sValue != string.Empty && ddl.Items[i].Text.Trim() == sValue)
                {
                    ddl.SelectedIndex = i;
                    break;
                }
            }

            lblMessage.Text = "";
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
            DataTable dt = Equ_SubjectDP.GetSubjects();
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
                        //arrTexts.Add(row["CatalogName"].ToString());
                        arrTexts.Add("");
                        arrValues.Add(lngRootID.ToString());
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
                    arrTexts.Add(PriLongEmptyString(Layer * 4) + row["CataLogName"].ToString());
                    arrValues.Add(row["catalogid"].ToString());
                    AddSubArrList(dt, long.Parse(row["catalogID"].ToString()), Layer + 1, ref arrTexts, ref arrValues);
                }
            }
        }
        #endregion

        #region 加空串 PriLongEmptyString
        /// <summary>
        /// 加空串
        /// </summary>
        /// <param name="len"></param>
        /// <returns></returns>
        private string PriLongEmptyString(int len)
        {
            string str = "";
            for (int i = 0; i < len; i++)
            {
                str += System.Web.HttpUtility.HtmlDecode("&nbsp;");
            }
            return str;
        }
        #endregion


    }
}