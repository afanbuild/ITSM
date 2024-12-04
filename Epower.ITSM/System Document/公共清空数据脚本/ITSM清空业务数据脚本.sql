Delete Equ_Category where CatalogID<>1;
Delete Inf_Category where CatalogID<>1;
Delete EA_ServicesTemplate where TemplateID<>-1;
commit;

TRUNCATE TABLE app_definedata;
TRUNCATE TABLE App_pub_Normal_Head;
truncate table BR_Attachment_Config;
TRUNCATE TABLE Br_Category;
TRUNCATE TABLE br_conditionsave;
TRUNCATE TABLE Br_Contact;
TRUNCATE TABLE Br_ECustomer;
TRUNCATE TABLE br_extendsfields;
TRUNCATE TABLE br_extensionsitems;
TRUNCATE TABLE Br_MastCustomer;
TRUNCATE TABLE br_meetingscheduled;
TRUNCATE TABLE br_messagerulinstall;
TRUNCATE TABLE br_orderclass;
TRUNCATE TABLE br_orderclasstype;
TRUNCATE TABLE br_rpt_engineer;
TRUNCATE TABLE br_schemaitems;
TRUNCATE TABLE br_send_email;
TRUNCATE TABLE clobstemptable;
TRUNCATE TABLE comeinjfcompertbl;
TRUNCATE TABLE comeinjfpeople;

TRUNCATE TABLE Cst_Byts;
TRUNCATE TABLE Cst_BytsRel;
TRUNCATE TABLE Cst_Cost;
TRUNCATE TABLE Cst_Issues;
TRUNCATE TABLE cst_issue_querysave;
TRUNCATE TABLE cst_recommendrule;
TRUNCATE TABLE cst_recommendruledetails;
TRUNCATE TABLE cst_Request;
TRUNCATE TABLE Cst_ServiceLevel;
TRUNCATE TABLE Cst_ServiceStaff;
TRUNCATE TABLE Cst_ServiceStaffList;
TRUNCATE TABLE Cst_SLGuid;
TRUNCATE TABLE desk_import_log;

TRUNCATE TABLE ea_code;
TRUNCATE TABLE EA_DefinePersonOpinion;
TRUNCATE TABLE EA_ExtendRights;
TRUNCATE TABLE Ea_FlowBusLimit;
TRUNCATE TABLE EA_FlowRel;
TRUNCATE TABLE EA_FlowRelConfig;
TRUNCATE TABLE EA_Idiom;
TRUNCATE TABLE EA_Issues_FeedBack;
TRUNCATE TABLE ea_mailsend;
TRUNCATE TABLE EA_Monitor;
TRUNCATE TABLE ea_operate;
TRUNCATE TABLE ea_operateobject;
TRUNCATE TABLE EA_PersonPlan;
TRUNCATE TABLE EA_PlanDetail;
TRUNCATE TABLE EA_PlanHistory;
TRUNCATE TABLE ea_printset;
TRUNCATE TABLE ea_services_attachment;
TRUNCATE TABLE ea_setdesk;
TRUNCATE TABLE EA_ShortCutTemplate;
TRUNCATE TABLE Ea_SmsRec;
TRUNCATE TABLE Ea_SmsSend;
TRUNCATE TABLE ea_sqlcachetableschange;
TRUNCATE TABLE ea_systemconfig;
TRUNCATE TABLE ea_visit;
TRUNCATE TABLE emailissue;


TRUNCATE TABLE equ_attachment;
TRUNCATE TABLE equ_catelists;
TRUNCATE TABLE Equ_ChangeService;
TRUNCATE TABLE equ_changeservicedetails;
TRUNCATE TABLE equ_deploy;
TRUNCATE TABLE equ_deployhistory;
TRUNCATE TABLE Equ_Desk;
TRUNCATE TABLE Equ_DeskChange;
TRUNCATE TABLE equ_deskchangedeploy;
TRUNCATE TABLE Equ_DeskDefineItem;
TRUNCATE TABLE Equ_DeskHistory;
TRUNCATE TABLE equ_monthsetting;
TRUNCATE TABLE Equ_PatrolItemData;
TRUNCATE TABLE Equ_PatrolPlanItem;
TRUNCATE TABLE Equ_PatrolService;
TRUNCATE TABLE Equ_Rel;
TRUNCATE TABLE equ_relname;
TRUNCATE TABLE equ_relpreference;
TRUNCATE TABLE Equ_SchemaItems;
TRUNCATE TABLE equ_weeksetting;
TRUNCATE TABLE EPOWER_SERVER_LOG;

TRUNCATE TABLE flow_questhouse;

TRUNCATE TABLE gs_curschedulesrule;
TRUNCATE TABLE gs_engineer_schedules;
TRUNCATE TABLE gs_preschedules;
TRUNCATE TABLE gs_restcategory;
TRUNCATE TABLE gs_rule_category;
TRUNCATE TABLE gs_schedulesarea;
TRUNCATE TABLE gs_schedules_base;
TRUNCATE TABLE gs_turn_detl;
TRUNCATE TABLE gs_turn_rule;

TRUNCATE TABLE hr_import_log;

TRUNCATE TABLE Inf_Attachment;
TRUNCATE TABLE Inf_BBS;
TRUNCATE TABLE Inf_Information;
TRUNCATE TABLE Inf_KMBase;
TRUNCATE TABLE Inf_Rel;
TRUNCATE TABLE Inf_Score;
TRUNCATE TABLE Inf_Tags;
TRUNCATE TABLE inf_transfer_set;
TRUNCATE TABLE inportexceltemp;
TRUNCATE TABLE itsm_hr_tbl;

TRUNCATE TABLE mailandmessagerule;
TRUNCATE TABLE mailandmessagetemplate;


TRUNCATE TABLE OA_AddressListFile;
TRUNCATE TABLE OA_Attachment;
TRUNCATE TABLE OA_Attention;
TRUNCATE TABLE OA_EmailNotify;
TRUNCATE TABLE OA_NEWS;
TRUNCATE TABLE OA_NEWS_TYPE;
TRUNCATE TABLE oa_releasemanagement;
TRUNCATE TABLE oa_releasesub;
TRUNCATE TABLE OA_Sms;
TRUNCATE TABLE printrule;

TRUNCATE TABLE Pro_ProblemAnalyse;
TRUNCATE TABLE Pro_ProblemDeal;
TRUNCATE TABLE pro_problemrel;
TRUNCATE TABLE Pro_ProvideManage;

TRUNCATE TABLE tryapplycustominfo;

commit;

