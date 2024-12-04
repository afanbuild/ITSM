/*******************************************************************
 * ��Ȩ���У�
 * Description��Ȩ�����õ�
 * 
 * 
 * Create By  ��
 * Create Date��
 * *****************************************************************/
namespace Epower.ITSM.Web.Controls
{
    using System;
    using System.Data;
    using System.Drawing;
    using System.Web;
    using System.Web.UI.WebControls;
    using System.Web.UI.HtmlControls;
    using Epower.DevBase.Organization;
    using Epower.DevBase.BaseTools;

    /// <summary>
    ///		CtrRight ��ժҪ˵����
    /// </summary>
    public partial class CtrRight : System.Web.UI.UserControl
    {

        private int _RightValue = 0;
        private bool _Enabled = true;

        public bool Enabled
        {
            set
            {
                _Enabled = value;
                chkCanAdd.Enabled = _Enabled;
                chkCanModify.Enabled = _Enabled;
                chkCanDelete.Enabled = _Enabled;
                chkCanRead.Enabled = _Enabled;
            }
            get
            {
                return _Enabled;
            }
        }

        public int RightValue
        {
            set
            {
                _RightValue = value;
                SetValue(_RightValue);
            }
            get
            {
                _RightValue = GetValue();
                return _RightValue;
            }
        }

        private void SetValue(int nRightValue)
        {
            CRight cr = new CRight(nRightValue);
            chkCanAdd.Checked = cr.CanAdd;
            chkCanRead.Checked = cr.CanRead;
            chkCanDelete.Checked = cr.CanDelete;
            chkCanModify.Checked = cr.CanModify;
        }

        private int GetValue()
        {
            CRight cr = new CRight();
            cr.CanAdd = chkCanAdd.Checked;
            cr.CanDelete = chkCanDelete.Checked;
            cr.CanModify = chkCanModify.Checked;
            cr.CanRead = chkCanRead.Checked;
            return StringTool.String2Int(cr.ToString());
        }
        protected void Page_Load(object sender, System.EventArgs e)
        {

        }

        #region Web ������������ɵĴ���
        override protected void OnInit(EventArgs e)
        {
            //
            // CODEGEN: �õ����� ASP.NET Web ���������������ġ�
            //
            InitializeComponent();
            base.OnInit(e);
        }

        /// <summary>
        ///		�����֧������ķ��� - ��Ҫʹ�ô���༭��
        ///		�޸Ĵ˷��������ݡ�
        /// </summary>
        private void InitializeComponent()
        {

        }
        #endregion
    }
}
