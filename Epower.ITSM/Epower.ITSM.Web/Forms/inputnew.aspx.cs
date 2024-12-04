using System;
using System.Collections;
using System.ComponentModel;
using System.Data;
using System.Data.OracleClient;
using System.Drawing;
using System.Web;
using System.Web.SessionState;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.HtmlControls;
using Epower.DevBase.BaseTools;
using Epower.ITSM.SqlDAL;
using FreeTextBoxControls;
using Epower.ITSM.Base;
using Epower.DevBase.Organization.Base;
using Epower.DevBase.Organization.SqlDAL;

namespace Epower.ITSM.Web.Forms
{
    /// <summary>
    /// inputnew 的摘要说明。
    /// </summary>
    public partial class inputnew : BasePage
    {
        #region 控件定义
        protected System.Web.UI.WebControls.Label LblSoftName;
        /// <summary>
        /// 
        /// </summary>
        protected long NewsId
        {
            get
            {
                if (ViewState["NewID"] != null)
                    return long.Parse(ViewState["NewID"].ToString());
                else
                    return 0;
            }
            set
            {
                ViewState["NewID"] = value;
            }
        }
        /// <summary>
        /// 
        /// </summary>
        protected long TNewsID
        {
            get
            {
                if (ViewState["TNewsID"] != null)
                    return long.Parse(ViewState["TNewsID"].ToString());
                else
                    return 0;
            }
            set
            {
                ViewState["TNewsID"] = value;
            }
        }
        #endregion


        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void Page_Load(object sender, System.EventArgs e)
        {
            TxtNewsTitle.Attributes.Add("onblur", "javascript:MaxLength(this,100,'输入信息题目长度超出限定长度：');");
            TxtNewsWriter.Attributes.Add("onblur", "javascript:MaxLength(this,40,'输入发布单位/人员长度超出限定长度：');");


            NewsId = StringTool.String2Int(Request.QueryString["NewsId"]);

            //this.Master.ShowBackUrlButton(true);
            this.Master.Master_Button_GoHistory_Click +=new Global_BtnClick(Master_Master_Button_GoHistory_Click);
            //labInput.Text = Session["PersonName"].ToString() + "[" + Session["UserDeptName"].ToString() + "]";

            if (!IsPostBack)
            {
                //设置显示
                PageDeal.SetLanguage(this.Controls[0]);

                if (NewsId != 0)
                {
                    TNewsID = NewsId;
                }
                else if (TNewsID == 0)
                {
                    TNewsID = EpowerGlobal.EPGlobal.GetNextID("OA_NewsId");  //生成公告ID
                }
                //FreeTextBox1.AutoConfigure = AutoConfigure.EnableAll;
                

                BindData();
                ListItem item = new ListItem("==请选择信息类型==", "0");
                this.DpDNewType.Items.Insert(0, item);
                LoadData();
            }
        }

        void Master_Master_Button_GoHistory_Click()
        {
            Response.Redirect(Session["FromUrl"].ToString());
        }

        private void LoadData()
        {
            if (StringTool.String2Int(Session["UserOrgID"].ToString()) == 1) //省局的人员
            {
                DrpIsInner.Items.Add(new ListItem("全体人员", ((int)eOA_ReadRange.AllRange).ToString()));

            }
            DrpIsInner.Items.Add(new ListItem("本单位人员", ((int)eOA_ReadRange.OrgRange).ToString()));
            DrpIsInner.Items.Add(new ListItem("本部门人员", ((int)eOA_ReadRange.DeptRange).ToString()));
            DrpIsInner.Items[0].Selected = true;

            if (NewsId != 0)
            {
                DataTable dt = NewsDp.GetNewsList(NewsId);

                if (StringTool.String2Int(dt.Rows[0]["FocusNews"].ToString()) == 1)
                    this.ChkFocusNews.Checked = true;

                if (StringTool.String2Int(dt.Rows[0]["IsBulletin"].ToString()) == 1)
                    this.chkBulletin.Checked = true;


                this.TxtNewsTitle.Text = dt.Rows[0]["Title"].ToString();

                ListItem lstitmNewType;
                lstitmNewType = this.DpDNewType.Items.FindByValue(dt.Rows[0]["TypeId"].ToString());
                if (lstitmNewType != null)
                {
                    this.DpDNewType.SelectedIndex = this.DpDNewType.Items.IndexOf(lstitmNewType);
                }

                ListItem lstitmDispFlag;
                lstitmDispFlag = this.DrpDispFlag.Items.FindByValue(dt.Rows[0]["DispFlag"].ToString());
                if (lstitmDispFlag != null)
                {
                    this.DrpDispFlag.SelectedIndex = this.DrpDispFlag.Items.IndexOf(lstitmDispFlag);
                }

                ListItem lstitmIsInner;
                lstitmIsInner = this.DrpIsInner.Items.FindByValue(dt.Rows[0]["IsInner"].ToString());
                if (lstitmIsInner != null)
                {
                    this.DrpIsInner.SelectedIndex = this.DrpIsInner.Items.IndexOf(lstitmIsInner);
                }

                this.TxtNewsWriter.Text = dt.Rows[0]["Writer"].ToString();
                this.ctrTxtNewsPubdate.dateTimeString = dt.Rows[0]["PubDate"].ToString();
                this.strtxtOutDate.dateTimeString = dt.Rows[0]["OutDate"].ToString();
                this.FreeTextBox1.Text = dt.Rows[0]["Content"].ToString();

                //新增是否弹出字段
                this.DrIsAlert.SelectedIndex = this.DrIsAlert.Items.IndexOf(this.DrIsAlert.Items.FindByValue(dt.Rows[0]["IsAlert"].ToString()));
                Ctrattachment1.AttachmentType = eOA_AttachmentType.eNews;
                Ctrattachment1.FlowID = NewsId;
            }
            else
            {
                this.ctrTxtNewsPubdate.dateTimeString = DateTime.Now.ToString("yyyy-MM-dd");
                this.strtxtOutDate.dateTimeString = DateTime.Now.AddMonths(1).ToString("yyyy-MM-dd");
            }
        }


        private bool CheckRight(long OperatorID)
        {
            //return ((RightDP.GetUserRight(OperatorID,UserID).RightValue & (int)eO_OperateRight.eCanRead) !=0);
            RightEntity re = (RightEntity)((Hashtable)Session["UserAllRights"])[OperatorID];
            if (re == null)
                return false;
            else
                return re.CanRead;
        }


        private void BindData()
        {
            DpDNewType.Items.Clear();
            DataTable dt = NewsDp.GetNewType();
            if (dt != null)
            {
                ListItem li;
                long lngUserID = StringTool.String2Long(Session["UserID"].ToString());
                dt.PrimaryKey = new DataColumn[] { dt.Columns["TypeId"] };

                foreach (DataRow dr in dt.Rows)
                {

                    li = new ListItem(dr["TypeName"].ToString(), dr["TypeId"].ToString());
                    DpDNewType.Items.Add(li);
                }


            }


        }

        protected void BtnSub_ok_Click(object sender, System.EventArgs e)
        {
            string UploadFileName = "";
            string strTmpFileName = "";
            if (this.DpDNewType.SelectedValue == "0")
            {
                labMsg.Text = "请选择信息类别";
                return;
            }
            else
            {
                labMsg.Text = "";
            }
            NewsEntity News = new NewsEntity();
            News.TNewsID = TNewsID;
            News.Title = this.TxtNewsTitle.Text.Trim();
            News.Writer = this.TxtNewsWriter.Text.Trim();
            News.TypeId = StringTool.String2Int(this.DpDNewType.SelectedItem.Value);
            News.PubDate = this.ctrTxtNewsPubdate.dateTimeString.Trim();
            News.OutDate = this.strtxtOutDate.dateTimeString.Trim();
            News.DispFlag = (eOA_DispFlag)StringTool.String2Int(this.DrpDispFlag.SelectedItem.Value);
            if (Session["PhotoName"] != null)
            {
                News.Photo = Session["PhotoName"].ToString();
                Session.Remove("PhotoName");

            }
            else
            {
                News.Photo = "";
            }

            if (this.ChkFocusNews.Checked)
                News.FocusNews = (int)eOA_IsFocus.eTrue;
            else
                News.FocusNews = (int)eOA_IsFocus.eFalse;

            if (this.chkBulletin.Checked)
                News.Bulletin = (int)eOA_IsBulletin.eTrue;
            else
                News.Bulletin = (int)eOA_IsBulletin.eFalse;


            News.Content = this.FreeTextBox1.Text;
            News.FileName = strTmpFileName;
            News.SoftName = UploadFileName;

            News.InDeptID = StringTool.String2Long(Session["UserDeptID"].ToString());
            News.InOrgID = StringTool.String2Long(Session["UserOrgID"].ToString());
            News.InputUser = StringTool.String2Long(Session["UserID"].ToString());
            News.NewsId = NewsId;
            News.IsInner = StringTool.String2Int(this.DrpIsInner.SelectedItem.Value);

            News.AttachXml = Ctrattachment1.AttachXML;
            //新增是否弹出
            News.IsAlert = decimal.Parse(DrIsAlert.SelectedItem.Value);
            News.Save();
            Response.Redirect("FrmNews_mng.aspx");
        }

        protected void btnCancel_Click(object sender, EventArgs e)
        {
            Response.Redirect("FrmNews_mng.aspx");
        }
    }
}
