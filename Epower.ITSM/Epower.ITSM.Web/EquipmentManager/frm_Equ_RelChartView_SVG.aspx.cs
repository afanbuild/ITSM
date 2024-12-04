

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

using System.Xml;
using System.Xml.Xsl;
using System.Xml.XPath;
using System.IO;

using Epower.ITSM.SqlDAL;
using Epower.ITSM.SqlDAL.EquipmentManager;
using System.Text;
using Epower.ITSM.SqlDAL.ResourceMoniter;

namespace Epower.ITSM.Web.EquipmentManager
{
    public partial class frm_Equ_RelChartView_SVG : BasePage
    {
        #region 变量

        #region ReadOnly 是否可在图上进行编辑
        /// <summary>
        /// 是否可在图上进行编辑
        /// </summary>
        protected string ReadOnly
        {
            get
            {
                if (Request["ReadOnly"] != null && Request["ReadOnly"].ToString() == "true")
                {
                    return "true";
                }
                else
                {
                    return "false";
                }
            }

        }
        #endregion

        #region Type 影响资产
        /// <summary>
        /// Type 资产关联于图
        /// </summary>
        protected string Type
        {
            get
            {
                if (Request["Type"] != null && Request["Type"].ToString() == "4")
                {
                    return "4";
                }
                else
                {
                    return "-1";
                }
            }

        }

        /// <summary>
        /// 资产监控状态刷新频率
        /// </summary>
        protected String Interval
        {
            get
            {
                return Utility.GetInterval();
            }
        }
        #endregion

        #endregion
        /// <summary>
        /// 用户偏好列表
        /// </summary>
        private System.Collections.Generic.List<Equ_RelPreference> listPrefer
        { get; set; }

        protected void Page_Load(object sender, EventArgs e)
        {
            Int32 intUserId = Int32.Parse(Session["UserId"].ToString());
            listPrefer = Equ_RelPreference.Load(intUserId);
            ///* 为用户导入初始偏好设置 */
            //Boolean isOk = Equ_RelPreference.CheckHave(intUserId);
            //if (!isOk) Equ_RelPreference.ImportData(intUserId);
            string strSearchKey = Request.QueryString["search_key"];
            if (String.IsNullOrEmpty(strSearchKey)) strSearchKey = "default";

            if (!IsPostBack)
            {
                BindRelKeyList();
                Boolean isOk = BindPreferList(strSearchKey);

                if (!isOk)
                {

                    string sXml = "";
                    long lngID = 0;
                    if (this.Request.QueryString["id"] != null)
                        lngID = long.Parse(Request.QueryString["id"]);


                    Equ_RelDP rdp = new Equ_RelDP();

                    if (String.IsNullOrEmpty(strSearchKey))
                        sXml = rdp.GetEquAllRelXml(lngID, 125, 500, 1300, 900, Type);
                    else
                        sXml = rdp.GetEquAllRelXmlByKey(lngID, 125, 500, 1300, 900, Type, strSearchKey);

                    Epower.DevBase.BaseTools.E8Logger.Info(sXml);

                    XmlDocument xmlDoc = new XmlDocument();

                    xmlDoc.LoadXml(sXml);


                    XPathNavigator nav = xmlDoc.DocumentElement.CreateNavigator();


                    XslTransform xmlXsl = new XslTransform();

                    xmlXsl.Load(Server.MapPath("../xslt/EquImage_SVG.xslt"));

                    XsltArgumentList xslArg = new XsltArgumentList();

                    //xslArg.AddParam("FinishedNodeID", "", "");
                    //xslArg.AddParam("CurrNodeID", "", -1);
                    TextWriter writer = new StringWriter();
                    //xmlXsl.Transform(nav, xslArg, Response.OutputStream);
                    xmlXsl.Transform(nav, xslArg, writer);
                    this.graph.Text = writer.ToString();

                }

            }


            if (Type == "4")
            {
                title.Text = "资产影响图";
            }

            
        }

        /// <summary>
        /// 绑定该用户的视角偏好
        /// </summary>        
        private void BindRelKeyList()
        {
            DataTable dt = Equ_RelPreference.LoadDataByUid(Int32.Parse(Session["UserId"].ToString()));

            Equ_RelNameDP equRelNameDP = new Equ_RelNameDP();
            DataTable dt2 = equRelNameDP.GetAll();

            StringBuilder sbText = new StringBuilder();
            String strTemplate = literalRelKeyList.Text;

            foreach (DataRow item in dt2.Rows)
            {
                Boolean isPass = true;
                foreach (DataRow item2 in dt.Rows)
                {
                    if (item["ID"].ToString().Equals(item2["ID"].ToString()))
                    {
                        isPass = false;
                        break;
                    }
                }

                if (!isPass) continue;

                sbText.AppendFormat(strTemplate,
                    item["ID"],
                    item["RelKey"]);
            }

            literalRelKeyList.Text = sbText.ToString();
        }
        /// <summary>
        /// 绑定所有视角
        /// </summary>
        /// <param name="strSearchKey">视角名</param>
        /// <returns>是否绑定</returns>
        private Boolean BindPreferList(String strSearchKey)
        {
            if (String.IsNullOrEmpty(strSearchKey))
                return false;
            if (!strSearchKey.Equals("prefer"))
                return false;

            Equ_RelNameDP equRelNameDP = new Equ_RelNameDP();
            DataTable dt = equRelNameDP.GetAll();

            dgPrefer.DataSource = dt;
            dgPrefer.DataBind();

            return true;
        }

        protected void btnSaveChanges_Click(object sender, EventArgs e)
        {
            Int32 intUserId = Int32.Parse(Session["UserId"].ToString());

            foreach (DataGridItem item in dgPrefer.Items)
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

            Response.Redirect(Request.Url.ToString());
        }

        protected void dgPrefer_ItemDataBound(object sender, DataGridItemEventArgs e)
        {
            if (e.Item.ItemType == ListItemType.AlternatingItem
                || e.Item.ItemType == ListItemType.Item)
            {

                CheckBox chkPrefer = e.Item.FindControl("chkprefer") as CheckBox;
                Int32 intRelKeyId = Int32.Parse(e.Item.Cells[0].Text);

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
    }
}
