/****************************************************************************
 * 
 * description:Í¼Æ¬Ñ¡Ôñ
 * 
 * 
 * 
 * Create by:
 * Create Date:2008-05-05
 * *************************************************************************/
using System;
using System.IO;
using System.Data;
using System.Configuration;
using System.Collections;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Web.UI.HtmlControls;

namespace Epower.ITSM.Web.EquipmentManager
{
    public partial class frmpopImage : BasePage
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {

                string sPath = Request.PhysicalApplicationPath + "Images\\EquImage";

                DirectoryInfo dir = new DirectoryInfo(sPath);
         

                if (dir != null)
                {
                    FileInfo[] files = dir.GetFiles("*.jpg");
                    string[] sArr= new string[files.Length];
                    for(int i=0;i< files.Length;i++)
                    {
                        sArr[i] = "../Images/EquImage/" + files[i].Name;
                    }
                    dtlstImage.DataSource = sArr;
                    dtlstImage.DataBind();
                }

               
            }
            
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="source"></param>
        /// <param name="e"></param>
        protected void dtlstImage_ItemCommand(object source, DataListCommandEventArgs e)
        {

        }
    }
}
