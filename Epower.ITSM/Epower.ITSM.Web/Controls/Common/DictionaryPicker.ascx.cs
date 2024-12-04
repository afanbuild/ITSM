/*******************************************************************
 * 版权所有：
 * Description：数据字典选取控件
 * 
 * 
 * Create By  ：
 * Create Date：
 * *****************************************************************/
namespace Epower.ITSM.Web.Controls
{
	using System;
	using System.Data;
	using System.Drawing;
	using System.Web;
	using System.Web.UI.WebControls;
	using System.Web.UI.HtmlControls;

	/// <summary>
	/// SimpleDictionaryPicker的摘要说明。
	/// </summary>
    public partial class DictionaryPicker : System.Web.UI.UserControl
	{
		#region Control & Private
		protected System.Web.UI.WebControls.DropDownList DrpDictionary;
        private long listID = 0;
        private long selectedid = 0;
		private string defaultItem = string.Empty;
		#endregion

        public event EventHandler mySelectedIndexChanged;

		#region Public Property
        public long SelectedID
		{
			get
			{
				if(DrpDictionary.Items.Count > 0)
                    return long.Parse(DrpDictionary.SelectedItem.Value);
				else
					return 0;
			}
			set
			{
                selectedid = value;
                //BindDrop();
			}
		}

        public string SelectedValue
        { 
            get
            {
                if(DrpDictionary.Items.Count > 0)
                    return DrpDictionary.SelectedItem.Text;
				else
					return string.Empty;
            }
        }

		public string DefaultItem
		{
			set
			{
				defaultItem = value;
			}
		}

        public long ListID
        {
            set
            {
                listID = value;
            }
        }

        public bool ReadOnly
        {
            set
            {
                DrpDictionary.Enabled = !value;
            }
        }

        public bool Visible
        {
            set
            {
                DrpDictionary.Visible = value;
            }
        }



    
		#endregion

		#region Events
		private void BindDrop()
		{
            DrpDictionary.DataSource = EpowerCom.FlowModel.GetDataDictionary(listID);
            DrpDictionary.DataTextField = "CName";
            DrpDictionary.DataValueField = "ItemID";
            DrpDictionary.DataBind();
            if (defaultItem != string.Empty)
                DrpDictionary.Items.Insert(0, new ListItem(defaultItem, "0"));
            if (DrpDictionary.Items.FindByValue(selectedid.ToString()) != null)
			{
				DrpDictionary.SelectedItem.Selected = false;
                DrpDictionary.Items.FindByValue(selectedid.ToString()).Selected = true;
			}
		}

		private void Page_Load(object sender, System.EventArgs e)
		{
            if (mySelectedIndexChanged != null)
            {
                DrpDictionary.AutoPostBack = true;
                DrpDictionary.SelectedIndexChanged += new EventHandler(DrpDictionary_SelectedIndexChanged);
            }
			if(!IsPostBack)
			{
				BindDrop();
			}
		}

        void DrpDictionary_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (mySelectedIndexChanged != null)

                mySelectedIndexChanged(this, new EventArgs()); //激活SelectIndexChanged事件 
        }

		#endregion

	
	}
}
