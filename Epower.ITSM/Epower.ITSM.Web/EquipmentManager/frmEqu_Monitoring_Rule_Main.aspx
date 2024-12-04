<%@ Page Language="C#" MasterPageFile="~/MasterPage.master" AutoEventWireup="true"
    CodeBehind="frmEqu_Monitoring_Rule_Main.aspx.cs" Inherits="Epower.ITSM.Web.EquipmentManager.frmEqu_Monitoring_Rule_Main"
    Title="资产监控规则配置" %>

<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <link type="text/css" href="../App_Themes/NewOldMainPage/css.css" rel="stylesheet" />
    <style type="text/css">
        .mark-main
        {
            width: 100%;
            height: 100%;
            margin-top: 10px;
            padding-left: 10px;
        }
        .mark-left
        {
            width: 20%;                  
            float: left;
        }
        .mark-right
        {
            float: left;
            margin-left: 7px;
            width: 78%;
        }
        #list
        {
            width: 100%;
        }
    </style>
    <div class="mark-main">
        <div class="mark-left">
            <iframe src="frmEqu_Content.aspx?Type=3" width="150" height="700" frameborder="0"></iframe>
        </div>
        <div class="mark-right">
            <iframe id="list" height="700" name="list" src="frmEqu_Monitoring_Rule_Resource_List.aspx" frameborder="0"></iframe>
        </div>
    </div>
</asp:Content>
