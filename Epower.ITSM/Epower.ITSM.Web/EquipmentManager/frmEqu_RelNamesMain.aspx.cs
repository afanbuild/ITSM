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
using Epower.ITSM.SqlDAL.EquipmentManager;

namespace Epower.ITSM.Web.EquipmentManager
{
    public partial class frmEqu_RelNamesMain : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                ActionProcess();

                BindRelKeyList();
            }
        }

        private void ActionProcess()
        {
            String strAction = Request.QueryString["action"];
            Int32 intRelKeyId;
            Boolean isOk = Int32.TryParse(Request.QueryString["relkeyid"], out intRelKeyId);

            if (!isOk) return;
            if (String.IsNullOrEmpty(strAction)) return;

            Equ_RelNameDP equRelNameDP = new Equ_RelNameDP();
            DataTable dt = equRelNameDP.GetAll();
            DataRow dr = null;

            foreach (DataRow item in dt.Rows)
            {
                if (item["ID"].ToString().Equals(intRelKeyId.ToString()))
                {
                    dr = item;
                    break;
                }
            }

            switch (strAction)
            {
                case "delete":


                    equRelNameDP.DeleteById(intRelKeyId, dr["RelKey"].ToString());
                    break;
                case "update":
                    txtRelKey.Text = dr["RelKey"].ToString();
                    hfRelKeyId.Value = intRelKeyId.ToString();

                    panel.Visible = true;
                    btnAddNew.Visible = false;
                    break;
                default:
                    break;
            }
        }

        /// <summary>
        /// 绑定所有视角
        /// </summary>        
        private void BindRelKeyList()
        {
            Equ_RelNameDP equRelNameDP = new Equ_RelNameDP();
            DataTable dt = equRelNameDP.GetAll();

            dgRelKey.DataSource = dt;
            dgRelKey.DataBind();
        }


        protected void dgRelKey_ItemDataBound(object sender, DataGridItemEventArgs e)
        {
            if (e.Item.ItemType == ListItemType.AlternatingItem
                || e.Item.ItemType == ListItemType.Item)
            {
                String strCmdText = e.Item.Cells[2].Text;
                Int32 intRelKeyId = Int32.Parse(e.Item.Cells[0].Text);

                e.Item.Cells[2].Text = String.Format(strCmdText, intRelKeyId);
            }
        }

        protected void btnAddNews_Click(object sender, EventArgs e)
        {
            Int32 intRelKeyId;
            Boolean isOk = Int32.TryParse(hfRelKeyId.Value, out intRelKeyId);

            Equ_RelNameDP equRelNameDP = new Equ_RelNameDP();
            //修改
            if (isOk)
            {
                equRelNameDP.UpdateRelName(intRelKeyId, txtRelKey.Text.Trim().ToLower());
            }
            else
            {
                //增加
                equRelNameDP.AddRelName(txtRelKey.Text.Trim().ToLower());
            }

            BindRelKeyList();

            panel.Visible = false;
            btnAddNew.Visible = true;
        }

        protected void btnAddNew_Click(object sender, EventArgs e)
        {
            panel.Visible = true;
            btnAddNew.Visible = false;
            txtRelKey.Text = String.Empty;
            hfRelKeyId.Value = String.Empty;
        }

        protected void btnCancel_Click(object sender, EventArgs e)
        {
            panel.Visible = false;
            btnAddNew.Visible = true;
        }
    }
}
