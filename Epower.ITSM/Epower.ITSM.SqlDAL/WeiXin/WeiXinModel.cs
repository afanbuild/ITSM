using System;
using System.Collections.Generic;
using System.Text;

namespace Epower.ITSM.SqlDAL
{
    public class WeiXinModel
    {
        private long _receiverid = 0;
        private long _messageid = 0;
        private long _flowmodelid = 0;
        private long _flowid = 0;
        private long _actionid = 0;
        private long _linkNodeID = 0;
        private long _linkNodeType = 0;
        private long _senderID = 0;
        private string _submitType = "send";
        private string _newAttURL = "";

        /// <summary>
        /// 构造函数
        /// </summary>
        public WeiXinModel()
        { 
        
        }
        /// <summary>
        /// 接收人员ID
        /// </summary>
        public long ReceiverID
        {
            get { return _receiverid; }
            set { _receiverid = value; }
        }
        /// <summary>
        /// 消息ID
        /// </summary>
        public long MessageID
        {
            get { return _messageid; }
            set { _messageid = value; }
        }
        /// <summary>
        /// 流程模型ID
        /// </summary>
        public long FlowmodelID
        {
            get { return _flowmodelid; }
            set { _flowmodelid = value; }
        }
        /// <summary>
        /// 流程ID
        /// </summary>
        public long FlowID
        {
            get { return _flowid; }
            set { _flowid = value; }
        }
        /// <summary>
        /// 环节按钮ID
        /// </summary>
        public long ActionID
        {
            get { return _actionid; }
            set { _actionid = value; }
        }
        /// <summary>
        /// 下一个环节ID
        /// </summary>
        public long LinkNodeID
        {
            get { return _linkNodeID; }
            set { _linkNodeID = value; }
        }
        /// <summary>
        /// 下一个环节类型
        /// </summary>
        public long LinkNodeType
        {
            get { return _linkNodeType; }
            set { _linkNodeType = value; }
        }
        /// <summary>
        /// 发送人员ID
        /// </summary>
        public long SenderID
        {
            get { return _senderID; }
            set { _senderID = value; }
        }
        /// <summary>
        /// 提交类型 send为发送流程 add为新增流程
        /// </summary>
        public string SubmitType
        {
            get { return _submitType; }
            set { _submitType = value; }
        }
        /// <summary>
        /// 新增附件的URL地址
        /// </summary>
        public string NewAttURL
        {
            get { return _newAttURL; }
            set { _newAttURL = value; }
        }
    }
}
