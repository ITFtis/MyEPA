# 督導業務

|表格代號|ContactManualSupervise|主鍵|Id|
|-|-|-|-|
|表格名稱|ContactManualSupervise|副鍵||
|功能說明|督導業務|索引||
|備註||Trigger||

|欄位說明||||||
|-|-|-|-|-|-|
|項次|欄位名稱|欄位說明|資料格式|允許null|備註（單位、限制、…）|
|1.|Id|主鍵|int|N||
|2.|DepartmentId|單位|int|N||
|3.|Describe|督導業務|nvarchar(max)|N||
|4.|CreateDate||datetime|N||
|5.|CreateUser||nvarchar(50)|N||
|6.|UpdateDate||datetime|N||
|7.|UpdateUser||nvarchar(50)|N||

# 手冊職務

|表格代號|ContactManualRole|主鍵|Id|
|-|-|-|-|
|表格名稱|ContactManualRole|副鍵||
|功能說明|手冊職務|索引||
|備註||Trigger||

|欄位說明||||||
|-|-|-|-|-|-|
|項次|欄位名稱|欄位說明|資料格式|允許null|備註（單位、限制、…）|
|1.|Id|主鍵|int|N||
|2.|Name|職務名稱|int|N||
|3.|Type|類別|int|N|0(一般),1(回收基管會)|
|4.|CreateDate||datetime|N||
|5.|CreateUser||nvarchar(50)|N||
|6.|UpdateDate||datetime|N||
|7.|UpdateUser||nvarchar(50)|N||

# 手冊值班日期

|表格代號|ContactManualDate|主鍵|Id|
|-|-|-|-|
|表格名稱|ContactManualDate|副鍵||
|功能說明|手冊值班日期|索引||
|備註||Trigger||

|欄位說明||||||
|-|-|-|-|-|-|
|項次|欄位名稱|欄位說明|資料格式|允許null|備註（單位、限制、…）|
|1.|Id|主鍵|int|N||
|2.|Date|值班日期|datetime2|N||
|3.|CreateDate||datetime|N||
|4.|CreateUser||nvarchar(50)|N||
|5.|UpdateDate||datetime|N||
|6.|UpdateUser||nvarchar(50)|N||

# 手冊單位

|表格代號|ContactManualDepartment|主鍵|Id|
|-|-|-|-|
|表格名稱|ContactManualDepartment|副鍵||
|功能說明|手冊單位|索引||
|備註||Trigger||

|欄位說明||||||
|-|-|-|-|-|-|
|項次|欄位名稱|欄位說明|資料格式|允許null|備註（單位、限制、…）|
|1.|Id|主鍵|int|N||
|2.|Name|單位名稱|nvarchar(200)|N||
|3.|Type|類別|int|N|0:一般<br>1:回收基管會<br>2:督察總隊<br>3:化學局24小時勤情單位|
|4.|CreateDate||datetime|N||
|5.|CreateUser||nvarchar(50)|N||
|6.|UpdateDate||datetime|N||
|7.|UpdateUser||nvarchar(50)|N||

# 手冊主表

|表格代號|ContactManual|主鍵|Id|
|-|-|-|-|
|表格名稱|ContactManual|副鍵||
|功能說明|手冊主表|索引||
|備註||Trigger||

|欄位說明||||||
|-|-|-|-|-|-|
|項次|欄位名稱|欄位說明|資料格式|允許null|備註（單位、限制、…）|
|1.|Id|主鍵|int|N||
|2.|Type|手冊資料類別|int|N||
|3.|SourceId|CityId,DepartmentId|int|N|儲存一些來源 id|
|4.|UserId| Users.Id|int|N||
|5.|Sort|排序|int|N||
|6.|Note|備註|nvarchar(200)|Y||
|7.|RoleId|手冊職務ID|int|Y||
|8.|CreateDate|新增時間|datetime|N||
|9.|CreateUser|新增人|nvarchar(50)|N||
|10.|UpdateDate|修改時間|datetime|N||
|11.|UpdateUser|修改人|nvarchar(50)|N||

# 手冊下載紀錄

|表格代號|ContactManualDownloadRecord|主鍵|Id|
|-|-|-|-|
|表格名稱|ContactManualDownloadRecord|副鍵||
|功能說明|手冊主表|索引||
|備註||Trigger||

|欄位說明||||||
|-|-|-|-|-|-|
|項次|欄位名稱|欄位說明|資料格式|允許null|備註（單位、限制、…）|
|1.|Id|主鍵|int|N||
|2.|UserId|下載 User|int|N||
|3.|Name|下載 User 姓名|nvarchar(50)|N||
|4.|MobilePhone|手機|nvarchar(50)|N||
|5.|CreateDate|新增時間|datetime|N||
|6.|CreateUser|新增人|nvarchar(50)|N||