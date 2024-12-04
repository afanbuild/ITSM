using System;
using System.Collections;
using System.Configuration;
using System.Data;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Web.UI.HtmlControls;
using System.Xml;
using Epower.ITSM.SqlDAL.ES_TBLCS;
using EpowerCom;

namespace Epower.ITSM.Web.FromES_TBl
{
    public partial class Frm_DefineLanguageList : BasePage
    {
        #region SetParentButtonEvent
        /// <summary>
        /// 
        /// </summary>
        protected void SetParentButtonEvent()
        {
            cpDefineLanguages.On_PageIndexChanged += new Epower.ITSM.Web.Controls.ControlPageFoot.ControlPageFootDelegate(LoadData);
            this.Master.Master_Button_Query_Click += new Global_BtnClick(Master_Master_Button_Query_Click);
            this.Master.Master_Button_Save_Click += new Global_BtnClick(Master_Master_Button_Save_Click);
            this.Master.ShowSaveButton(true);
            this.Master.ShowQueryButton(true);
        }
        #endregion
        
        protected void Page_Load(object sender, EventArgs e)
        {
            SetParentButtonEvent();
            cpDefineLanguages.On_PageIndexChanged = new Epower.ITSM.Web.Controls.ControlPageFoot.ControlPageFootDelegate(LoadData);
            if (!IsPostBack)
            {
                InitDropDownList();                
                LoadData();
            }
        }

        #region 初始化查询分组的下拉列表
        /// <summary>
        /// 初始化查询分组的下拉列表
        /// </summary>
        protected void InitDropDownList()
        {
            DataTable dt = ES_TBL.GetGroups();

            ddlGroup.DataTextField = "Groups";
            ddlGroup.DataValueField = "Groups";
            ddlGroup.DataSource = dt.DefaultView;
            ddlGroup.DataBind();

            ddlGroup.Items.Insert(0, new ListItem("","0"));
        }
        #endregion

        #region Master_Master_Button_Query_Click
        /// <summary>
        /// 查询
        /// </summary>
        protected void Master_Master_Button_Query_Click()
        {
            this.cpDefineLanguages.CurrentPage = 1;
            LoadData();
        }
        #endregion

        #region 生成查询XML字符串 GetXmlValue
        /// <summary>
        /// 生成查询XML字符串 GetXmlValue
        /// </summary>
        /// <returns></returns>
        protected XmlDocument GetXmlValue()
        {
            FieldValues fv = new FieldValues();

            fv.Add("Group", ddlGroup.SelectedValue);
            fv.Add("KeyValue", txtKeyVa.Text.ToString().Trim());

            XmlDocument xmlDoc = fv.GetXmlObject();
            return xmlDoc;
        }
        #endregion

        #region LoadData
        /// <summary>
        /// 
        /// </summary>
        protected void LoadData()
        {
            int iRowCount = 0;
            XmlDocument xmlDoc = GetXmlValue();
            DataTable dt = ES_TBL.GetDefineLanguageLists(xmlDoc.InnerXml, "", this.cpDefineLanguages.PageSize, this.cpDefineLanguages.CurrentPage, ref iRowCount);
            dgDefineLanguages.DataSource = dt.DefaultView;
            dgDefineLanguages.DataBind();
            this.cpDefineLanguages.RecordCount = iRowCount;
            this.cpDefineLanguages.Bind();
        }
        #endregion

        #region Master_Master_Button_Save_Click
        /// <summary>
        /// 保存
        /// </summary>
        protected void Master_Master_Button_Save_Click()
        {
            foreach (DataGridItem item in dgDefineLanguages.Items)
            {
                if (item.ItemType == ListItemType.Item || item.ItemType == ListItemType.AlternatingItem)
                {
                    string strID = item.Cells[0].Text;          //ID
                    string strOrgKV = ((TextBox)item.FindControl("txtKeyValue")).Text;      //信息项名称
                    //根据ID更新信息项名称
                    ES_TBL.UptKeyValueByID(strID, strOrgKV);
                }
            }

            HttpRuntime.Cache.Remove("EA_DefineLanguageCache");
        }
        #endregion

        #region dgDefineLanguages_ItemDataBound
        /// <summary>
        /// dgDefineLanguages_ItemDataBound
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void dgDefineLanguages_ItemDataBound(object sender, DataGridItemEventArgs e)
        {
            if (e.Item.ItemType == ListItemType.Item || e.Item.ItemType == ListItemType.AlternatingItem)
            {
                e.Item.Attributes.Add("onmouseover", "currentcolor=this.style.backgroundColor;this.style.backgroundColor='#FFFBE8'");
                e.Item.Attributes.Add("onmouseout", "this.style.backgroundColor=currentcolor");
            }
        }
        #endregion 
    }
}
