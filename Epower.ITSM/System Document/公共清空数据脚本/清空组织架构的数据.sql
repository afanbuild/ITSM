delete ts_dept where deptid<>1;
delete ts_user where userid<>1;
commit;

TRUNCATE TABLE ts_actorcond;
TRUNCATE TABLE ts_actormembers;
TRUNCATE TABLE ts_actors;
TRUNCATE TABLE ts_rights;
TRUNCATE TABLE ts_userdept;
TRUNCATE TABLE ts_user_temp;
TRUNCATE TABLE Ts_IPRanges;
commit;