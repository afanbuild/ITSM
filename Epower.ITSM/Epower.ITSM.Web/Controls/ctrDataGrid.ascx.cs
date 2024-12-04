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
using System.Collections.Generic;

namespace Epower.ITSM.Web.Controls
{
    public partial class ctrDataGrid : System.Web.UI.UserControl
    {

        #region 必设属性

        /// <summary>
        /// 模块名
        /// </summary>
        public String ModuleName
        {
            get
            {
                Object objModuleName = ViewState["ModuleName"];
                if (objModuleName == null) return String.Empty;

                return objModuleName.ToString();
            }
            set
            {
                ViewState["ModuleName"] = value;
            }
        }

        /// <summary>
        /// 自定义项分组名
        /// </summary>
        public String EADefineLanguageGroup
        {
            get
            {
                Object objGroup = ViewState["EA_Define_Language_Group"];
                if (objGroup == null) return String.Empty;

                return objGroup.ToString();
            }
            set { ViewState["EA_Define_Language_Group"] = value; }
        }

        /// <summary>
        /// 数据源表
        /// </summary>
        private DataTable _dataSource;

        #endregion

        /// <summary>
        /// 设置数据源
        /// </summary>
        /// <param name="dataSource"></param>
        public void SetDataSource(DataTable dataSource)
        {
            SetColumns();

            this.dgDataList.DataSource = dataSource;
            this.dgDataList.DataBind();

            this._dataSource = dataSource;
        }

        private void SetColumns()
        {
            /*
             * 
             * subject:100&name:20&
             * 
             * **/

            //List<KeyValuePair<String, String>> listColumn = new List<KeyValuePair<string, string>>();

            //listColumn.Add(new KeyValuePair<string, string>("problem_typename", "52%"));
            //listColumn.Add(new KeyValuePair<string, string>("problem_subject", "52px"));

            //List<List<String>> listColumn = new List<List<string>>();
            //listColumn.AddRange(new String[] { "problem_typename", "52", "%" });
            //listColumn.AddRange(new String[] { "problem_subject", "12", "%" });
            //listColumn.AddRange(new String[] { "problem_happendTime", "33", "px" });


            //DataTable dtNew = new DataTable();

            //foreach (KeyValuePair<String, String> item in listColumn)
            //{
            //    BoundColumn boundColumn = new BoundColumn();
            //    boundColumn.DataField = item.Key;
            //    boundColumn.HeaderText = PageDeal.GetLanguageValue(item.Key, this.EADefineLanguageGroup);
            //    boundColumn.HeaderStyle.Width = new Unit(12, UnitType.Pixel);

            //    this.dgDataList.Columns.Add(boundColumn);
            //}
        }

        protected void dgProblem_DeleteCommand(object source, DataGridCommandEventArgs e)
        {

        }

        protected void dgProblem_ItemCreated(object sender, DataGridItemEventArgs e)
        { }

        protected void dgProblem_ItemDataBound(object sender, DataGridItemEventArgs e)
        { }

        protected void Page_Load(object sender, EventArgs e)
        {

        }
    }
}